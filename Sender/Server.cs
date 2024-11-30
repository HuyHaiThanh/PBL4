using System;
using System.Threading;
using UdpFileTransfer;
using System.Threading.Tasks;
using System.Net;

namespace Sender
{
    public class Server
    {
        public static UdpFileSender fileSender;

        // Xử lý dừng truyền file khi nhấn Ctrl-C
        public static void InterruptHandler(object sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;
            fileSender?.Shutdown();
        }

        public static void Main(string[] args)
        {
            // Setup the sender
            string filesDirectory = "Files";
            int port = 6000;
            fileSender = new UdpFileSender(filesDirectory, port);

            // Tạo một luồng để xử lý thông điệp mạng
            //Task.Run(() =>
            //{
            //    Console.WriteLine("Waiting for messages...");
            //    fileSender.HandleMessage();
            //});

            // Thử nhận yêu cầu ping từ client
            //Console.WriteLine("Waiting for ping request...");
            //fileSender.StartServer();

            // Xử lý yêu cầu danh sách file ở luồng khác
            Task.Run(() =>
            {
                fileSender.HandleListFileRequest();
            });


            //// Thêm sự kiện Ctrl-C
            Console.CancelKeyPress += InterruptHandler;
                
            //// Bắt đầu truyền file
            fileSender.Run();  // Bắt đầu truyền file
                
            Console.WriteLine("Press Ctrl-C to stop the sender.");
            fileSender.Close(); // Đóng kết nối
        }
    }
}
