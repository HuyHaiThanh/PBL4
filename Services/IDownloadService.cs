using System.Threading.Tasks;
using PBL4.Models;

namespace PBL4.Services
{
    // Interface của DownloadService
    public interface IDownloadService
    {
        Task DownloadFileAsync(DownloadTask downloadtask); // Phương thức download file
    }
}