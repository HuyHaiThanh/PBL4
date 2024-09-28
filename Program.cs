using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using PBL4.Controllers;
using PBL4.Services;
using PBL4.Utilities;

namespace PBL4
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Cấu hình dịch vụ và logging
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Lấy controller từ DI
            var downloadController = serviceProvider.GetRequiredService<DownloadController>();

            // Nhập thông tin từ người dùng hoặc từ đối số dòng lệnh
            Console.WriteLine("Nhập URL tệp tin cần tải về:");
            string url = Console.ReadLine();

            Console.WriteLine("Nhập thư mục lưu tệp tin (ví dụ: C:\\Downloads):");
            string outputDirectory = Console.ReadLine();

            Console.WriteLine("Nhập số lượng phần tải song song (mặc định 4):");
            string partsInput = Console.ReadLine();
            int numberOfParts = 4;
            if (!string.IsNullOrEmpty(partsInput) && int.TryParse(partsInput, out int parts))
            {
                numberOfParts = parts;
            }

            // Tạo thư mục lưu trữ nếu chưa tồn tại
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Bắt đầu tải xuống
            await downloadController.StartDownloadAsync(url, outputDirectory, numberOfParts);

            Console.WriteLine("Nhấn bất kỳ phím nào để kết thúc.");
            Console.ReadKey();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            // Cấu hình logging
            services.AddLogging(configure => 
            {
                configure.AddConsole();
                configure.SetMinimumLevel(LogLevel.Information);
            });

            // Cấu hình IHttpClientFactory
            services.AddHttpClient();

            // Đăng ký các dịch vụ
            services.AddSingleton<IDownloadService, DownloadService>();
            services.AddSingleton<DownloadController>();
        }
    }
}
