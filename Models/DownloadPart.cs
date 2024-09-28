namespace PBL4.Models
{
    // Một phần của file cần download
    public class DownloadPart
    {
        public int PartNumber { get; set; } // Số thứ tự của phần
        public long From { get; set; } // Byte bắt đầu của phần
        public long To { get; set; } // Byte kết thúc của phần
        public byte[] Data { get; set; } // Dữ liệu của phần
    }
}
