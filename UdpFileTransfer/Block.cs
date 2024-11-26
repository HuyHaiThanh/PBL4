using System;
using System.Text;
using System.Linq;

namespace UdpFileTransfer
{
    // Khối dữ liệu
    public class Block
    {
        // Số thứ tự
        public UInt32 Number { get; set; }

        // Dữ liệu
        public byte[] Data { get; set; } = new byte[0];

        #region Constructors

        // Khởi tạo mặc định
        public Block(UInt32 number = 0)
        {
            Number = number;
        }

        // Khởi tạo từ mảng byte
        public Block(byte[] bytes)
        {
            // 4 byte đầu là số thứ tự
            Number = BitConverter.ToUInt32(bytes, 0);

            // Các byte còn lại là dữ liệu
            Data = bytes.Skip(4).ToArray();
        }
        #endregion // Constructors

        public override string ToString()
        {
            // Lấy một số byte đầu tiên và chuyển thành chuỗi   
            String dataStr;
            if (Data.Length > 8)
                dataStr = Encoding.ASCII.GetString(Data, 0, 8) + "..."; // Lấy 8 byte đầu
            else
                dataStr = Encoding.ASCII.GetString(Data, 0, Data.Length); // Lấy hết

            // Trả về chuỗi theo định dạng: [Block: Number=..., Size=..., Data=...]
            return string.Format(
                "[Block:\n" +
                "  Number={0},\n" +
                "  Size={1},\n" +
                "  Data=`{2}`]",
                Number, Data.Length, dataStr);
        }

        // Chuyển khối dữ liệu thành mảng byte
        public byte[] GetBytes()
        {
            // Chuyển số thứ tự thành mảng byte (4 byte đầu)
            byte[] numberBytes = BitConverter.GetBytes(Number);

            // Ghép mảng byte (các byte còn lại)
            byte[] bytes = new byte[numberBytes.Length + Data.Length];
            numberBytes.CopyTo(bytes, 0);
            Data.CopyTo(bytes, 4);

            return bytes;
        }
    }
}