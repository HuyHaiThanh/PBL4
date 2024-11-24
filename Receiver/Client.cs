﻿using System;
using System.Threading;
using UdpFileTransfer;

namespace Receiver
{
    public class Client
    {
        public static UdpFileReceiver fileReceiver;

        // Xử lý dừng nhận file khi nhấn Ctrl-C
        public static void InterruptHandler(object sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;
            fileReceiver?.Shutdown();
        }

        public static void Main(string[] args)
       {
            // Cấu hình nhận file
            string hostname = "127.0.0.1"; // IP server 
            int port = 6000;

            fileReceiver = new UdpFileReceiver(hostname, port);

            // Kiểm tra kết nối
            //if (!fileReceiver.Connect())
            //{
            //    Console.WriteLine("Không thể kết nối đến server.");
            //    return; 
            //}

            // Ping server trước khi tiếp tục
            //bool pingSuccess = fileReceiver.Ping(hostname, port);
            //if (!pingSuccess)
            //{
            //    Console.WriteLine("Không thể kết nối đến server.");
            //    return;
            //}

            Console.Write("Nhập tên file cần nhận: ");
            string filename = Console.ReadLine();

            //
            //Thêm sự kiện Ctrl-C để dừng nhận file
            Console.CancelKeyPress += InterruptHandler;
            //
            //// Bắt đầu nhận file
            fileReceiver.GetFile(filename);
            fileReceiver.Close();
        }
    }
}
