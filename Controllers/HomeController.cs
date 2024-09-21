using Microsoft.AspNetCore.Mvc;
using PBL4_SERVER.Models;
using System.Diagnostics;

namespace PBL4_SERVER.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Dowload()
        {
            return View();
        }
        public IActionResult DownloadFile(string fileName)
        {
            fileName = "test.txt";
            // Đường dẫn đầy đủ đến file trong thư mục C:\Files
            var filePath = Path.Combine("F:\\Files", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); // Nếu file không tồn tại
            }

            // Đọc file từ đường dẫn
            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            // Trả về file để tải xuống
            return File(fileBytes, "text/plain", fileName);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
