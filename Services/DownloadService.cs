using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PBL4.Utilities;
using PBL4.Models;


namespace PBL4.Services
{
    public class DownloadService : IDownloadService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<DownloadService> _logger;

        public DownloadService(IHttpClientFactory httpClientFactory, ILogger<DownloadService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task DownloadFileAsync(DownloadTask task)
        {
            var client = _httpClientFactory.CreateClient();

            // Gửi yêu cầu HEAD để lấy thông tin file
            var headRequest = new HttpRequestMessage(HttpMethod.Head, task.Url);
            var headResponse = await client.SendAsync(headRequest);
            headResponse.EnsureSuccessStatusCode();

            // Xác định tên file từ Content-Disposition nếu có
            if (headResponse.Content.Headers.ContentDisposition != null &&
                !string.IsNullOrEmpty(headResponse.Content.Headers.ContentDisposition.FileName))
            {
                task.FileName = headResponse.Content.Headers.ContentDisposition.FileName.Trim('"');
            }
            else
            {
                // Nếu không có Content-Disposition, lấy tên file từ URL
                task.FileName = Path.GetFileName(new Uri(task.Url).LocalPath);
            }

            // Nếu tên file trống, đặt tên mặc định
            if (string.IsNullOrEmpty(task.FileName))
            {
                task.FileName = "downloaded_file";
            }

            // Đảm bảo đường dẫn lưu trữ tồn tại
            string directory = Path.GetDirectoryName(task.OutputPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Đặt lại OutputPath với tên file đúng
            task.OutputPath = Path.Combine(directory, task.FileName);

            // Lấy kích thước tệp tin
            long totalSize = headResponse.Content.Headers.ContentLength ?? throw new Exception("Không thể lấy kích thước tệp tin.");

            long partSize = totalSize / task.NumberOfParts;
            var tasksList = new List<Task<DownloadPart>>();

            for (int i = 0; i < task.NumberOfParts; i++)
            {
                long from = partSize * i;
                long to = (i == task.NumberOfParts - 1) ? totalSize - 1 : (from + partSize - 1);
                var part = new DownloadPart
                {
                    PartNumber = i + 1,
                    From = from,
                    To = to
                };
                task.Parts.Add(part);
                tasksList.Add(DownloadPartAsync(client, task.Url, part));
            }

            var downloadedParts = await Task.WhenAll(tasksList);

            // Ghi các phần vào tệp tin đầu ra
            using (FileStream fs = new FileStream(task.OutputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                foreach (var part in downloadedParts)
                {
                    await fs.WriteAsync(part.Data, 0, part.Data.Length);
                }
            }

            _logger.LogInformation($"Tải xuống hoàn tất: {task.OutputPath}");
        }

        private async Task<DownloadPart> DownloadPartAsync(HttpClient client, string url, DownloadPart part)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(part.From, part.To);

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            using (var ms = new MemoryStream())
            {
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    await stream.CopyToAsync(ms);
                }
                part.Data = ms.ToArray();
                _logger.LogInformation($"Đã tải phần {part.PartNumber} từ {part.From} đến {part.To}");
                return part;
            }
        }
    }
}
