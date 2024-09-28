using PBL4.Controllers;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PBL4.ViewModels
{
    public class DownloadViewModel : INotifyPropertyChanged
    {
        private readonly DownloadController _downloadController;

        public DownloadViewModel(DownloadController downloadController)
        {
            _downloadController = downloadController;
        }

        // Thuộc tính và lệnh cho giao diện người dùng
        // Ví dụ: Tiến độ tải xuống, lệnh bắt đầu tải, v.v.

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Thêm các thuộc tính và phương thức khác tùy theo yêu cầu giao diện
    }
}
