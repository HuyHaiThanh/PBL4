using PBL4.Models;
using System.Collections.Generic;

namespace PBL4.Models
{
    public class DownloadTask
    {
        public string Url { get; set; }
        public string OutputPath { get; set; }
        public string FileName { get; set; } // Thêm thuộc tính tên file
        public int NumberOfParts { get; set; } = 4; // Mặc định 4 phần
        public List<DownloadPart> Parts { get; set; } = new List<DownloadPart>();
    }
}
