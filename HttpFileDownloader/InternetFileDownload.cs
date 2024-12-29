using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HttpFileDownloader
{
    public class InternetFileDownload : BaseDownloader
    {
        private FileDetail currentFile;

        private long fileSize;
        private long totalBytesDownloaded;


        private readonly string url;
        private readonly string outputFolder;
        private int numThreads;


        public InternetFileDownload(string url, string outputFolder, int numberOfParts)
        {

            this.url = url;
            this.outputFolder = outputFolder;
            this.numThreads = numberOfParts;
        }
        public async Task DownloadFileAsync()
        {
            try
            {
                Uri uri = new Uri(url);
                Observer.Instance.Broadcast(EventId.OnProcessDownloadStart, FileDownLoadStatus.Connect);
                if (!await SupportsRangeRequestAsync(uri))
                    numThreads = 1;
                string fileName = await GetFileName(url);
                fileSize = await GetFileSizeAsync(url);
                if(fileSize  ==  0)
                {
                    throw new ArgumentException("Không đọc được dữ liệu của file");
                }

                string outputPath = Path.Combine(outputFolder, fileName);
                currentFile = new FileDetail(fileName, fileSize / (1024 * 1024), numThreads);
                long chunkSize = fileSize / numThreads;

                var tasks = new List<Task>();
                var tempFiles = new List<string>();

                

                for (int i = 0; i < numThreads; i++)
                {
                    long startByte = i * chunkSize;
                    long endByte = (i == numThreads - 1) ? fileSize - 1 : (startByte + chunkSize - 1);
                    string tempFile = Path.Combine(outputFolder, $"part_{i}.tmp");
                    tempFiles.Add(tempFile);
                    tasks.Add(DownloadPartAsync(uri, startByte, endByte, tempFile));
                }

                await Task.WhenAll(tasks);

                MergeFileChunks(tempFiles, outputPath);
                Console.WriteLine("Download complete!");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Observer.Instance.Broadcast(EventId.OnHasError, message);
            }
        }

        public async Task<long> GetFileSizeAsync(string url)
        {
            try
            {
                HttpClient client = new HttpClient();
                using (var request = new HttpRequestMessage(HttpMethod.Head, url))
                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    return response.Content.Headers.ContentLength ?? throw new InvalidOperationException("Cannot determine file size.");
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Observer.Instance.Broadcast(EventId.OnHasError, message);
                return 0;
            }

        }


        private async Task<bool> SupportsRangeRequestAsync(Uri uri)
        {
            try
            {
                using (TcpClient tcpClient = new TcpClient(uri.Host, uri.Scheme == "https" ? 443 : 80))
                {
                    using (Stream stream = uri.Scheme == "https"
                        ? (Stream)new SslStream(tcpClient.GetStream(), false)
                        : tcpClient.GetStream())
                    {
                        if (stream is SslStream sslStream)
                            await sslStream.AuthenticateAsClientAsync(uri.Host);

                        // Tạo yêu cầu HEAD
                        string request = $"HEAD {uri.PathAndQuery} HTTP/1.1\r\nHost: {uri.Host}\r\nConnection: close\r\n\r\n";
                        byte[] requestBytes = Encoding.ASCII.GetBytes(request);
                        await stream.WriteAsync(requestBytes, 0, requestBytes.Length);

                        byte[] buffer = new byte[2048];
                        StringBuilder responseBuilder = new StringBuilder();
                        int bytesRead;

                        // Đọc phản hồi từ server cho đến khi hết header
                        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            responseBuilder.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
                            if (responseBuilder.ToString().Contains("\r\n\r\n"))
                            {
                                break;
                            }
                        }

                        string response = responseBuilder.ToString();

                        // Kiểm tra header Accept-Ranges
                        return response.Contains("Accept-Ranges: bytes");
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Observer.Instance.Broadcast(EventId.OnHasError, message);
                return false;
            }
        }


        private DateTime timeStartDown;
        bool isFirstThread = true;
        private async Task DownloadPartAsync(Uri uri, long startByte, long endByte, string tempFile)
        {
            try
            {
                using (TcpClient tcpClient = new TcpClient(uri.Host, uri.Scheme == "https" ? 443 : 80))
                {
                    using (Stream stream = uri.Scheme == "https"
                        ? (Stream)new SslStream(tcpClient.GetStream(), false)
                        : tcpClient.GetStream())
                    {
                        if (stream is SslStream sslStream)
                            await sslStream.AuthenticateAsClientAsync(uri.Host);

                        string request =
                            $"GET {uri.PathAndQuery} HTTP/1.1\r\n" +
                            $"Host: {uri.Host}\r\n" +
                            $"Range: bytes={startByte}-{endByte}\r\n" +
                            "Connection: close\r\n\r\n";
                        byte[] requestBytes = Encoding.ASCII.GetBytes(request);
                        await stream.WriteAsync(requestBytes, 0, requestBytes.Length);

        
                        if (isFirstThread)
                        {
                            isFirstThread = false;
                            timeStartDown = DateTime.Now;
                        }
                        using (FileStream fileStream = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
                        {
                            bool headerParsed = false;
                            byte[] buffer = new byte[8192];
                            int bytesRead;

                            DateTime startTime = DateTime.Now;
                            DateTime lastUpdate = DateTime.Now;

                            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                if (isCancelled)
                                {
                                    fileStream.Close();
                                    File.Delete(tempFile);
                                    return;
                                }
                                while (isPaused)
                                {
                                    await Task.Delay(100);
                                }
                                if (!headerParsed)
                                {
                                    string responseHeader = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                                    int headerEndIndex = responseHeader.IndexOf("\r\n\r\n");
                                    if (headerEndIndex != -1)
                                    {
                                        headerParsed = true;
                                        int bodyStartIndex = headerEndIndex + 4;
                                        int bodyBytes = bytesRead - bodyStartIndex;
                                        await fileStream.WriteAsync(buffer, bodyStartIndex, bodyBytes);
                                        totalBytesDownloaded += bodyBytes;
                                        double percentage = (double)totalBytesDownloaded / fileSize * 100;
                                        TimeSpan elapsedTime = DateTime.Now - startTime;
                                        double downloadSpeed = totalBytesDownloaded / elapsedTime.TotalSeconds;

                                        if ((DateTime.Now - lastUpdate).TotalMilliseconds >= 500)
                                        {
                                            lastUpdate = DateTime.Now;

                                            long downloadedMegaBytes = totalBytesDownloaded;
                                            double downloadSpeedMegaBytesPerSeconds = downloadSpeed / (1024 * 1024);


                                            currentFile.UpdateInfo((int)percentage, downloadedMegaBytes, downloadSpeedMegaBytesPerSeconds, FileDownLoadStatus.Downloading);
                                            Observer.Instance.Broadcast(EventId.OnProcessDownloadProgress, currentFile);
                                        }
                                    }
                                }
                                else
                                {
                                    if (isCancelled)
                                    {
                                        fileStream.Close();
                                        File.Delete(tempFile);
                                        return;
                                    }
                                    while (isPaused)
                                    {
                                        await Task.Delay(100);
                                    }
                                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                                    totalBytesDownloaded += bytesRead;
                                    double percentage = (double)totalBytesDownloaded / fileSize * 100;
                                    TimeSpan elapsedTime = DateTime.Now - startTime;
                                    double downloadSpeed = totalBytesDownloaded / elapsedTime.TotalSeconds;

                                    if ((DateTime.Now - lastUpdate).TotalMilliseconds >= 500)
                                    {
                                        lastUpdate = DateTime.Now;

                                        long downloadedMegaBytes = totalBytesDownloaded;
                                        double downloadSpeedMegaBytesPerSeconds = downloadSpeed / (1024 * 1024);


                                        currentFile.UpdateInfo((int)percentage, downloadedMegaBytes, downloadSpeedMegaBytesPerSeconds, FileDownLoadStatus.Downloading);
                                        Observer.Instance.Broadcast(EventId.OnProcessDownloadProgress, currentFile);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Observer.Instance.Broadcast(EventId.OnHasError, message);
            }
        }

        private void MergeFileChunks(List<string> tempFiles, string outputPath)
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
                string message = ex.Message;
                Observer.Instance.Broadcast(EventId.OnHasError, message);
            }


        }

        public async Task<string> GetFileName(string url)
        {
            try
            {
                // Tạo đối tượng Uri từ URL
                Uri uri = new Uri(url);

                using (TcpClient tcpClient = new TcpClient(uri.Host, uri.Scheme == "https" ? 443 : 80))
                {
                    // Lấy luồng mạng (SSL nếu HTTPS)
                    Stream stream = uri.Scheme == "https"
                        ? (Stream)new SslStream(tcpClient.GetStream(), false)
                        : tcpClient.GetStream();

                    if (stream is SslStream sslStream)
                        await sslStream.AuthenticateAsClientAsync(uri.Host);

                    // Gửi yêu cầu HTTP HEAD
                    string request = $"HEAD {uri.PathAndQuery} HTTP/1.1\r\nHost: {uri.Host}\r\nConnection: close\r\n\r\n";
                    byte[] requestBytes = Encoding.ASCII.GetBytes(request);
                    await stream.WriteAsync(requestBytes, 0, requestBytes.Length);

                    // Đọc phản hồi từ server
                    byte[] buffer = new byte[4096];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    // Kiểm tra nếu có tiêu đề Content-Disposition
                    string contentDispositionHeader = "Content-Disposition: ";
                    int dispositionIndex = response.IndexOf(contentDispositionHeader, StringComparison.OrdinalIgnoreCase);

                    if (dispositionIndex != -1)
                    {
                        int fileNameStart = response.IndexOf("filename=", dispositionIndex, StringComparison.OrdinalIgnoreCase) + 9;
                        if (fileNameStart > 9)
                        {
                            int fileNameEnd = response.IndexOf("\r\n", fileNameStart);
                            string fileName = response.Substring(fileNameStart, fileNameEnd - fileNameStart).Trim('"');
                            return fileName;
                        }
                    }

                    // Nếu không có Content-Disposition, trả về tên file từ URL
                    return Path.GetFileName(uri.LocalPath) ?? "downloaded_file";
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Observer.Instance.Broadcast(EventId.OnHasError, message);
                return null;
            }
        }


        public override void OnDispose()
        {
            throw new NotImplementedException();
        }
    }
}
