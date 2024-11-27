using Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Http
{
    public class HttpFileDownloader : BaseDowloader
    {
        private readonly HttpClient client;


        private FileDetail currentFile;

        private long fileSize;
        private long totalBytesDownloaded;


        private readonly string url;
        private readonly string outputFolder;
        private readonly int numThreads;


        public HttpFileDownloader(string url, string outputFolder, int numberOfParts)
        {
            client = new HttpClient();

            this.url = url;
            this.outputFolder = outputFolder;
            this.numThreads = numberOfParts;
        }

        public async Task<long> GetFileSizeAsync(string url)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Head, url))
                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    return response.Content.Headers.ContentLength ?? throw new InvalidOperationException("Cannot determine file size.");
                }
            }
            catch (HttpRequestException ex)
            {
                // Lỗi liên quan đến HTTP (kết nối thất bại, server trả lỗi, v.v.)
                Console.WriteLine($"HTTP error: {ex.Message}");
                throw;
            }
            catch (TaskCanceledException ex)
            {
                // Lỗi do hết thời gian chờ (timeout)
                Console.WriteLine("Request timed out.");
                throw new TimeoutException("The request timed out.", ex);
            }
            catch (Exception ex)
            {
                // Bắt các lỗi khác
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }

        }

        public async Task<string> GetFileName(string url)
        {
            try
                {
                    HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
                    response.EnsureSuccessStatusCode();

                    if (response.Content.Headers.ContentDisposition?.FileName != null)
                    {
                        return response.Content.Headers.ContentDisposition.FileName.Trim('"');
                    }

                    Uri uri = new Uri(url);
                    return Path.GetFileName(uri.LocalPath) ?? "downloaded_file";
             }
            catch (HttpRequestException ex)
            {
                // Lỗi liên quan đến HTTP (kết nối thất bại, server trả lỗi, v.v.)
                Console.WriteLine($"HTTP error: {ex.Message}");
                throw;
            }
            catch (TaskCanceledException ex)
            {
                // Lỗi do hết thời gian chờ (timeout)
                Console.WriteLine("Request timed out.");
                throw new TimeoutException("The request timed out.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving file name: " + ex.Message, ex);
            }
        }


        public async Task StartDownload()
        {
            Observer.Instance.Broadcast(EventId.OnProcessDownloadStart, FileDownLoadStatus.Connect);
            fileSize = await GetFileSizeAsync(url);
            string fileName = await GetFileName(url);
            string outputPath = Path.Combine(outputFolder, fileName);

            currentFile = new FileDetail(fileName, fileSize / (1024 * 1024), numThreads);     
            long partSize = fileSize / numThreads;

            var tasks = new List<Task>();
            var tempFiles = new List<string>();

            for (int i = 0; i < numThreads; i++)
            {
                long start = i * partSize;
                long end = (i == numThreads - 1) ? fileSize - 1 : (start + partSize - 1);
                string tempFile = Path.Combine(outputFolder, $"part_{i}.tmp");
                tempFiles.Add(tempFile);

                tasks.Add(DownloadPartAsync(url, tempFile, start, end));
            }

            try
            {
                await Task.WhenAll(tasks);
                MergeFiles(tempFiles, outputPath);
            }
            catch (Exception ex)
            {
                throw new Exception("Download failed: " + ex.Message, ex);
            }
        }
        private DateTime timeStartDown;
        bool isFirstThread = true;
        private async Task DownloadPartAsync(string url, string tempFile, long start, long end)
        {
            try
            {
                client.DefaultRequestHeaders.Range = new System.Net.Http.Headers.RangeHeaderValue(start, end);
                using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    if (isFirstThread)
                    {
                        isFirstThread = false;
                        timeStartDown = DateTime.Now;
                    }
                    using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                    fileStream = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                    {
                        byte[] buffer = new byte[8192];
                        int bytesRead;

                        DateTime startTime = DateTime.Now;
                        DateTime lastUpdate = DateTime.Now;

                        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {

                            if (isCancelled)
                            {
                                fileStream.Close();
                                File.Delete(tempFile);
                                return;
                            }
                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                            totalBytesDownloaded += bytesRead;

                            while (isPaused)
                            {
                                await Task.Delay(100);
                            }

                            double percentage = (double)totalBytesDownloaded / fileSize * 100;
                            TimeSpan elapsedTime = DateTime.Now - startTime;
                            double downloadSpeed = totalBytesDownloaded / elapsedTime.TotalSeconds;

                            if ((DateTime.Now - lastUpdate).TotalMilliseconds >= 500)
                                {
                                    lastUpdate = DateTime.Now;

                                    long downloadedMegaBytes = totalBytesDownloaded / (1024 * 1024);
                                    double downloadSpeedMegaBytesPerSeconds = downloadSpeed / (1024 * 1024);


                                currentFile.UpdateInfo((int)percentage, downloadedMegaBytes, downloadSpeedMegaBytesPerSeconds, FileDownLoadStatus.Downloading);
                                Observer.Instance.Broadcast(EventId.OnProcessDownloadProgress, currentFile);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error downloading part [{start}-{end}]: " + ex.Message, ex);
                }
        }

        private void MergeFiles(List<string> tempFiles, string outputPath)
        {
            if (isCancelled) return;
            currentFile.UpdateInfo(100, totalBytesDownloaded / (1024 * 1024), 0, FileDownLoadStatus.Merging);
            Observer.Instance.Broadcast(EventId.OnProcessDownloadProgress, currentFile);

            try
            {
                using (FileStream output = new FileStream(outputPath, FileMode.Create))
                {
                    foreach (string tempFile in tempFiles)
                    {
                        using (FileStream input = new FileStream(tempFile, FileMode.Open))
                        {
                            input.CopyTo(output);
                        }
                        File.Delete(tempFile);
                    }
                }
                double downloadTime = (DateTime.Now - timeStartDown).TotalSeconds;
                Observer.Instance.Broadcast(EventId.OnProcessDownloadCompleted, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Error merging files: " + ex.Message, ex);
            }

        }

        public override void OnDispose()
        {
            
        }
    }
}
