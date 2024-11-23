using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdpFileTransfer
{
    [Serializable]
    public class FileDetail
    {
        // Tên file
        public string FileName { get; set; }

        // Kích thước file
        public long FileSize { get; set; }

        // Hàm dựng
        public FileDetail(string fileName, long fileSize)
        {
            FileName = fileName;
            FileSize = fileSize;
        }
    }
}
