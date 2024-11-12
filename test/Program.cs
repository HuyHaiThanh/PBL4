using System.Threading;
using test;

class Program
{
    static void Main(string[] args)
    {
        int portR = 12345; // Port của Receiver
        int portS = 54321; // Port của Sender

        // Khởi tạo Receiver
        UdpStringReceiver receiver = new UdpStringReceiver(portR);

        // Khởi tạo Sender
        UdpStringSender sender = new UdpStringSender(portS);

        // Bắt đầu lắng nghe trên một luồng riêng cho Receiver
        Thread receiverThread = new Thread(() =>
        {
            receiver.StartListening();
        });
        receiverThread.Start();

        // Đợi một chút để Receiver sẵn sàng
        Thread.Sleep(1000);

        // Gửi thông điệp từ Sender đến Receiver
        sender.Send("Hello, are you there?", "localhost", portR);

        // Cho chương trình thời gian để nhận và hiển thị phản hồi
        Thread.Sleep(3000);
    }
}