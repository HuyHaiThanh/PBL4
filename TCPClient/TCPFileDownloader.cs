using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using Core;

namespace TCP
{
    public class TCPFileDownloader : BaseDowloader
    {
        private string ip;
        private int port;
        private string fileName;
        private string filePath;
        private int numThreads;

        private long fileSize;
        private long totalBytesDownloaded;



        private FileDetail currentFile;

        public TCPFileDownloader(string serverIP, int serverPort)
        {
            ip = serverIP;
            port = serverPort;
        }
        public TCPFileDownloader(string serverIP, int serverPort, string fileName, string filePath, int numThreads)
        {
            ip = serverIP;
            port = serverPort;
            this.fileName = fileName;
            this.filePath = filePath;
            this.numThreads = numThreads;
        }
        public List<FileDetail> GetAvailableFiles()
        {
            List<FileDetail> fileList = new List<FileDetail>();
            try
            {
                using (TcpClient client = new TcpClient(ip, port))
                using (NetworkStream stream = client.GetStream())
                using (StreamWriter writer = new StreamWriter(stream))
                using (StreamReader reader = new StreamReader(stream))
                {
                    writer.WriteLine(Constraint.listKey);
                    writer.Flush();

                    string response;
                    while ((response = reader.ReadLine()) != Constraint.endKey)
                    {
                        FileDetail fileDetail = Utilities.JsonToFileDetail(response);
                        fileList.Add(fileDetail);
                    }
                }

                return fileList;
            }
            catch(SocketException ex)
            {
                string message = ex.Message;
                Observer.Instance.Broadcast(EventId.OnHasError, message);
            }
            catch (IOException ex)
            {
                string message = ex.Message;
                Observer.Instance.Broadcast(EventId.OnHasError, message);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Observer.Instance.Broadcast(EventId.OnHasError, message);
            }
            return null;

        }
        private async Task<long> GetFileSizeAsync()
        {
            try
            {
                using (TcpClient client = new TcpClient(ip, port))
                {
                    using (NetworkStream stream = client.GetStream())
                    using (StreamWriter writer = new StreamWriter(stream))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        writer.WriteLine($"SIZE {fileName}");
                        await writer.FlushAsync();

                        string response = await reader.ReadLineAsync();
                        return long.Parse(response);
                    }
                }
            }
            catch (SocketException ex)
            {
                string message = ex.Message;
                Observer.Instance.Broadcast(EventId.OnHasError, message);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Observer.Instance.Broadcast(EventId.OnHasError, message);
            }
            return -1;

        }
        public async Task StartDownload()
        {

            Observer.Instance.Broadcast(EventId.OnProcessDownloadStart, FileDownLoadStatus.Connect);
            fileSize = await GetFileSizeAsync();
            if (fileSize == -1) return;

            currentFile = new FileDetail(fileName, fileSize / (1024 * 1024) , numThreads);
            long partSize = fileSize / numThreads;
            

            Task[] downloadTasks = new Task[numThreads];
            List<string> tempFiles = new List<string>();

            for (int i = 0; i < numThreads; i++)
            {
                long start = i * partSize;
                long end = (i == numThreads - 1) ? fileSize - 1 : (start + partSize - 1);

                string tempFile = $"part_{i}.tmp";
                tempFiles.Add(tempFile);

                downloadTasks[i] = Task.Run(() => DownloadPartAsync(start, end, tempFile));
            }

            await Task.WhenAll(downloadTasks);
            await MergePartsAsync(tempFiles);
        }


        private DateTime timeStartDown;
        bool isFirstThread = true;
        private async Task DownloadPartAsync(long start, long end, string tempFile)
        {
            using (TcpClient client = new TcpClient(ip, port))
            using (NetworkStream stream = client.GetStream())
            using (StreamWriter writer = new StreamWriter(stream))
            using (FileStream fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                if (isFirstThread)
                {
                    isFirstThread = false;
                    timeStartDown = DateTime.Now;
                }
                writer.WriteLine($"GET {fileName} {start} {end}");
                await writer.FlushAsync();
                byte[] buffer = new byte[8192];
                int bytesRead;
                DateTime startTime = DateTime.Now;
                DateTime lastUpdate = DateTime.Now;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    if (isCancelled)
                    {
                        fs.Close();
                        File.Delete(tempFile);
                        return;
                    }
                    await fs.WriteAsync(buffer, 0, bytesRead);
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

        private async Task MergePartsAsync(List<string> tempFiles)
        {
            if (isCancelled) return;
            currentFile.UpdateInfo(100, totalBytesDownloaded / (1024 * 1024), 0, FileDownLoadStatus.Merging);
            Observer.Instance.Broadcast(EventId.OnProcessDownloadProgress, currentFile);

            using (FileStream output = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                foreach (string tempFile in tempFiles)
                {
                    using (FileStream input = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
                    {
                        await input.CopyToAsync(output);
                    }

                    File.Delete(tempFile);
                }
            }
            double downloadTime = (DateTime.Now - timeStartDown).TotalSeconds;
            
            Observer.Instance.Broadcast(EventId.OnProcessDownloadCompleted, null);
        }


        public override void OnDispose()
        {
            
        }
    }
}