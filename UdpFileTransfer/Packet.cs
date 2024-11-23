using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace UdpFileTransfer
{
    // Gói tin
    public class Packet
    {
        // Các loại gói tin (Static)
        #region Messge Types (Static)

        // Danh sách file
        public static UInt32 ListFile = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("LIST"), 0);

        // Xác nhận dữ liệu gửi thành công
        public static UInt32 Ack = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("ACK "), 0);

        // Thoát, kết thúc truyền file
        public static UInt32 Bye = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("BYE "), 0);

        // Yêu cầu danh sách file
        public static UInt32 RequestList = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("REQL"), 0);

        // Yêu cầu file (cần ACK)
        public static UInt32 RequestFile = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("REQF"), 0);

        // Yêu cầu khối
        public static UInt32 RequestBlock = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("REQB"), 0); // Chứa số thứ tự khối

        // Thông tin file cần gửi (cần ACK)
        public static UInt32 Info = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("INFO"), 0); // Chứa MD5 checksum, kích thước file, kích thước khối, số lượng khối

        // Dữ liệu gửi
        public static UInt32 Send = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("SEND"), 0); // Chứa khối dữ liệu

        // Thứ tự: REQF -> ACK -> INFO -> ACK -> (REQB -> SEND)^n -> BYE -> BYE
        #endregion

        // Loại gói tin
        public UInt32 PacketType { get; set; } 

        // Dữ liệu
        public byte[] Payload { get; set; } = new byte[0];
                
        #region Handy Properties
        // Kiểm tra loại gói tin
        public bool IsListFile { get { return PacketType == ListFile; } }
        public bool IsAck { get { return PacketType == Ack; } }
        public bool IsBye { get { return PacketType == Bye; } }
        public bool IsRequestList { get { return PacketType == RequestList; } }
        public bool IsRequestFile { get { return PacketType == RequestFile; } }
        public bool IsRequestBlock { get { return PacketType == RequestBlock; } }
        public bool IsInfo { get { return PacketType == Info; } }
        public bool IsSend { get { return PacketType == Send; } }
        public bool IsUnknown { get { return !(IsAck || IsBye || IsRequestFile || IsRequestBlock || IsInfo || IsSend || IsRequestList || IsListFile); } }

        // Loại gói tin dưới dạng chuỗi
        public string MessageTypeString { get { return Encoding.UTF8.GetString(BitConverter.GetBytes(PacketType)); } }
        #endregion

        #region Constructors
        // Khởi tạo mặc định
        public Packet(UInt32 packetType)
        {
            // Loại gói tin
            PacketType = packetType;
        }

        // Khởi tạo từ mảng byte
        public Packet(byte[] bytes)
        {
            PacketType = BitConverter.ToUInt32(bytes, 0);   // Lấy 4 byte đầu là loại gói tin

            // Các byte còn lại là dữ liệu
            Payload = new byte[bytes.Length - 4];
            bytes.Skip(4).ToArray().CopyTo(Payload, 0);
        }
        #endregion // Constructors

        // Hiển thị thông tin gói tin dưới dạng chuỗi
        public override string ToString()
        {
            // Lấy một số byte đầu tiên và chuyển thành chuỗi
            String payloadStr;
            int payloadSize = Payload.Length;
            if (payloadSize > 8)
                payloadStr = Encoding.ASCII.GetString(Payload, 0, 8) + "..."; // Lấy 8 byte đầu
            else
                payloadStr = Encoding.ASCII.GetString(Payload, 0, payloadSize); // Lấy hết

            String typeStr = "UKNOWN"; // Loại gói tin mặc định
            if (!IsUnknown)
                typeStr = MessageTypeString; // Lấy loại gói tin dưới dạng chuỗi

            // Trả về chuỗi theo định dạng: [Packet: Type=..., PayloadSize=..., Payload=...]
            return string.Format(
                "[Packet:\n" +
                "  Type={0},\n" +
                "  PayloadSize={1},\n" +
                "  Payload=`{2}`]",
                typeStr, payloadSize, payloadStr);
        }

        // Chuyển gói tin thành mảng byte
        public byte[] GetBytes()
        {
            // 4 byte đầu là loại gói tin
            byte[] bytes = new byte[4 + Payload.Length];
            BitConverter.GetBytes(PacketType).CopyTo(bytes, 0);
            Payload.CopyTo(bytes, 4); // Các byte còn lại là dữ liệu

            return bytes;
        }
    }

    #region Definite Packets
    // REQL - Yêu cầu danh sách file
    // Chứa thông điệp (mã hóa UTF-8) và dữ liệu (nếu có)
    public class RequestListPacket : Packet
    {
        public string Message
        {
            get { return Encoding.UTF8.GetString(Payload); }
            set { Payload = Encoding.UTF8.GetBytes(value); }
        }

        // Khởi tạo mặc định
        public RequestListPacket(Packet p = null) :
            base(RequestList)
        {
            if (p != null)
                Payload = p.Payload;
        }
    }

    // LIST - Danh sách file
    // Chứa danh sách bao gồm tên file và dung lượng
    [Serializable]
    public class FileListPacket : Packet
    {
        public List<FileDetail> FileList
        {
            get
            {
                var memoryStream = new MemoryStream(Payload);
                var formatter = new BinaryFormatter();
                return (List<FileDetail>)formatter.Deserialize(memoryStream);
            }
            set
            {
                var memoryStream = new MemoryStream();
                var formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, value);
                Payload = memoryStream.ToArray();
            }
        }

        // Khởi tạo mặc định
        public FileListPacket(Packet p = null) :
            base(ListFile)
        {
            if (p != null)
                Payload = p.Payload;
        }

        // Hàm dựng
        public FileListPacket(List<FileDetail> fileList) :
            base(ListFile)
        {
            FileList = fileList;
        }
    }

    // ACK - Xác nhận
    // Chứa thông điệp (mã hóa UTF-8) và dữ liệu (nếu có)
    public class AckPacket : Packet
    {
        public string Message
        {
            get { return Encoding.UTF8.GetString(Payload); }
            set { Payload = Encoding.UTF8.GetBytes(value); }
        }

        // Khởi tạo mặc định
        public AckPacket(Packet p = null) :
            base(Ack)
        {
            if (p != null)
                Payload = p.Payload;
        }
    }

    // REQF - Yêu cầu file 
    // Chứa tên file (mã hóa UTF-8) và dữ liệu (nếu có)
    public class RequestFilePacket : Packet
    {
        public string Filename
        {
            get { return Encoding.UTF8.GetString(Payload); }
            set { Payload = Encoding.UTF8.GetBytes(value); }
        }

        public RequestFilePacket(Packet p = null) :
            base(RequestFile)
        {
            if (p != null)
                Payload = p.Payload;
        }

    }

    // REQB - Yêu cầu khối
    // Chứa số thứ tự khối và dữ liệu (nếu có)
    public class RequestBlockPacket : Packet
    {
        public UInt32 Number
        {
            get { return BitConverter.ToUInt32(Payload, 0); }
            set { Payload = BitConverter.GetBytes(value); }
        }

        public RequestBlockPacket(Packet p = null)
            : base(RequestBlock)
        {
            if (p != null)
                Payload = p.Payload;
        }
    }

    // INFO - Thông tin file
    // Chứa MD5 checksum, kích thước file, kích thước khối, số lượng khối
    public class InfoPacket : Packet
    {
        // Checksum (16 byte đầu)
        public byte[] Checksum
        {
            get { return Payload.Take(16).ToArray(); }
            set { value.CopyTo(Payload, 0); }
        }

        // Kích thước file (4 byte tiếp theo)
        public UInt32 FileSize
        {
            get { return BitConverter.ToUInt32(Payload.Skip(16).Take(4).ToArray(), 0); }
            set { BitConverter.GetBytes(value).CopyTo(Payload, 16); }
        }

        // Kích thước khối (4 byte tiếp theo)
        public UInt32 MaxBlockSize
        {
            get { return BitConverter.ToUInt32(Payload.Skip(16 + 4).Take(4).ToArray(), 0); }
            set { BitConverter.GetBytes(value).CopyTo(Payload, 16 + 4); }
        }

        // Số lượng khối (4 byte tiếp theo)
        public UInt32 BlockCount
        {
            get { return BitConverter.ToUInt32(Payload.Skip(16 + 4 + 4).Take(4).ToArray(), 0); }
            set { BitConverter.GetBytes(value).CopyTo(Payload, 16 + 4 + 4); }
        }

        // Khởi tạo mặc định
        public InfoPacket(Packet p = null)
            : base(Info)
        {
            if (p != null)
                Payload = p.Payload;
            else
                Payload = new byte[16 + 4 + 4 + 4];
        }
    }

    // SEND - Gửi dữ liệu
    // Chứa khối dữ liệu
    public class SendPacket : Packet
    {
        public Block Block
        {
            get { return new Block(Payload); }
            set { Payload = value.GetBytes(); }
        }

        // Khởi tạo mặc định
        public SendPacket(Packet p = null)
            : base(Send)
        {
            if (p != null)
                Payload = p.Payload;
        }
    }

    #endregion // Định nghĩa các gói tin cụ thể
}