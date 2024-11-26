using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Core;

namespace TcpServer
{
    class TCPFileServer
    {
        private string _directoryPath;
        private int _port;

        public TCPFileServer(string directoryPath, int port)
        {
            _directoryPath = directoryPath;
            _port = port;
        }

        public void StartServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, _port);
            listener.Start();
            Console.WriteLine($"Server listening on port {_port}");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Task.Run(() => HandleClient(client));
            }
        }

        private void HandleClient(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            using (StreamReader reader = new StreamReader(stream))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                string request = reader.ReadLine();
                string[] requestParts = request.Split(' ');

                if (requestParts[0] == "SIZE")
                {
                    Console.Write("has");
                    string fileName = requestParts[1];
                    Console.WriteLine($"{fileName}");
                    string filePath = Path.Combine(_directoryPath, fileName);

                    if (File.Exists(filePath))
                    {

                        long fileSize = new FileInfo(filePath).Length;
                        writer.WriteLine(fileSize);
                        Console.WriteLine(fileSize.ToString());
                        writer.Flush();
                    }
                    else
                    {
                        writer.WriteLine("ERROR: File not found");
                        writer.Flush();
                    }
                }
                else if (requestParts[0] == "GET")
                {
                    Console.WriteLine("get chuyen file");
                    string fileName = requestParts[1];
                    long start = long.Parse(requestParts[2]);
                    long end = long.Parse(requestParts[3]);

                    string filePath = Path.Combine(_directoryPath, fileName);

                    if (File.Exists(filePath))
                    {
                        SendFilePart(filePath, stream, start, end);
                        Console.WriteLine("Dang chuyen file");
                    }
                    else
                    {
                        writer.WriteLine("ERROR: File not found");
                        writer.Flush();
                    }
                }
                else if (requestParts[0] == "LIST")
                {

                    DirectoryInfo directoryInfo = new DirectoryInfo(_directoryPath);
                    FileInfo[] files = directoryInfo.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        FileDetail fileDetail = new FileDetail(file.Name, file.Length);

                        Console.WriteLine(Path.Combine(_directoryPath, file.Name));
                        string json = Utilities.FileDetailToJson(fileDetail);
                        writer.WriteLine(json);
                    }
                    writer.WriteLine("END");  // Ký hiệu để client biết kết thúc danh sách
                    writer.Flush();
                }
            }

            client.Close();
        }

        private void SendFilePart(string filePath, NetworkStream stream, long start, long end)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    fs.Seek(start, SeekOrigin.Begin);
                    byte[] buffer = new byte[8192];
                    long remaining = end - start + 1;

                    while (remaining > 0)
                    {
                        int bytesRead = fs.Read(buffer, 0, (int)Math.Min(buffer.Length, remaining));
                        if (bytesRead <= 0) break;

                        stream.Write(buffer, 0, bytesRead);
                        stream.Flush();
                        remaining -= bytesRead;
                    }
                }

                Console.WriteLine($"Sent file part from {start} to {end}");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"IOException: {ioEx.Message}");
                // Điều này xảy ra khi client đóng kết nối đột ngột
            }
            catch (SocketException socketEx)
            {
                Console.WriteLine($"SocketException: {socketEx.Message}");
                // Điều này xảy ra khi kết nối bị đóng bất ngờ (forcefully closed)
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                // Bắt mọi ngoại lệ khác
            }
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            string directoryPath = "F:\\Files";
            int port = 9000;

            TCPFileServer server = new TCPFileServer(directoryPath, port);
            server.StartServer();
        }
    }
}
