using System.Net;

namespace UdpFileTransfer
{
    // Thông điệp mạng
    public class NetworkMessage
    {
        // Địa chỉ nguồn
        public IPEndPoint Sender { get; set; }

        // Gói tin
        public Packet Packet { get; set; }
    }
}   