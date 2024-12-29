using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace HttpFileDownloader
{
    public class FtpFileDownloader : BaseDownloader
    {
        private readonly string ftpUrl;
        private readonly string outputFolder;
        private int numThreads;

        private FileDetail currentFile;
        private long fileSize;
        private long totalBytesDownloaded;
        private DateTime timeStartDown;
        private bool isFirstThread = true;

        public FtpFileDownloader(string ftpUrl, string outputFolder, int numberOfParts)
        {
            this.ftpUrl = ftpUrl;
            this.outputFolder = outputFolder;
            this.numThreads = numberOfParts;
        }

        public async Task<long> GetFileSizeAsync(string ftpUrl)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                {
                    return response.ContentLength;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting file size: {ex.Message}");
                throw;
            }
        }

        public string GetFileName(string ftpUrl)
        {
            Uri uri = new Uri(ftpUrl);
            return Path.GetFileName(uri.LocalPath) ?? "downloaded_file";
        }

        public async Task StartDownload()
        {
            Observer.Instance.Broadcast(EventId.OnProcessDownloadStart, FileDownLoadStatus.Connect);
            fileSize = await GetFileSizeAsync(ftpUrl);
            string fileName = GetFileName(ftpUrl);
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

                tasks.Add(DownloadPartAsync(ftpUrl, tempFile, start, end));
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

        private async Task DownloadPartAsync(string ftpUrl, string tempFile, long start, long end)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.ContentOffset = start;

                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                using (Stream responseStream = response.GetResponseStream())
                using (FileStream fileStream = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                {
                    byte[] buffer = new byte[8192];
                    int bytesRead;

                    DateTime startTime = DateTime.Now;
                    DateTime lastUpdate = DateTime.Now;

                    while ((bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        if (isFirstThread)
                        {
                            isFirstThread = false;
                            timeStartDown = DateTime.Now;
                        }

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
            // Implement any necessary cleanup here
        }
    }
}
