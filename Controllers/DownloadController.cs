using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PBL4.Models;
using PBL4.Services;

namespace PBL4.Controllers
{
    public class DownloadController
    {
        private readonly IDownloadService _downloadService;
        private readonly ILogger<DownloadController> _logger;

        public DownloadController(IDownloadService downloadService, ILogger<DownloadController> logger)
        {
            _downloadService = downloadService;
            _logger = logger;
        }

        public async Task StartDownloadAsync(string url, string outputDirectory, int numberOfParts = 4)
        {
            var downloadTask = new DownloadTask
            {
                Url = url,
                OutputPath = outputDirectory,
                NumberOfParts = numberOfParts
            };

            try
            {
                await _downloadService.DownloadFileAsync(downloadTask);
                _logger.LogInformation("Download completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi trong quá trình tải xuống: {ex.Message}");
            }
        }
    }
}
