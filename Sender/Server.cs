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

                //// Thêm sự kiện Ctrl-C
                Console.CancelKeyPress += InterruptHandler;
            
                // Thử nhận yêu cầu ping từ client
                //Console.WriteLine("Waiting for ping request...");
                //fileSender.StartServer();

                //// Bắt đầu truyền file
                fileSender.Init(); // Lấy danh sách file
                fileSender.Run();  // Bắt đầu truyền file

                Console.WriteLine("Press Ctrl-C to stop the sender.");
                fileSender.Close(); // Đóng kết nối
            }
        }
    }
