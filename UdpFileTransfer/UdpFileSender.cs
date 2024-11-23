using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Threading;
using System.Text;
using System.Threading.Tasks;

namespace UdpFileTransfer
{
    public class UdpFileSender
    {
        #region Statics
        // Kích thước tối đa của khối dữ liệu (8KB)
        public static readonly UInt32 MaxBlockSize = 8 * 1024;
        #endregion // Statics

        // Trạng thái của trình gửi
        enum SenderState
        {
            NotRunning, // Không chạy
            WaitingForFileRequest, // Chờ yêu cầu file
            PreparingFileForTransfer, // Chuẩn bị file để gửi
            SendingFileInfo, // Gửi thông tin file
            WaitingForInfoACK, // Chờ ACK từ người nhận
            Transfering // Đang gửi
        }

        // Dữ liệu mạng 
        private UdpClient _client;
        public readonly int Port; 

        // Trạng thái chạy
        public bool Running { get; private set; } = false;

        // Thư mục chứa file
        public readonly string FilesDirectory;

        // Danh sách file có thể gửi
        private HashSet<string> _transferableFiles;

        // Danh sách khối dữ liệu
        private Dictionary<UInt32, Block> _blocks = new Dictionary<UInt32, Block>();

        // Hàng đợi thông điệp mạng
        private Queue<NetworkMessage> _packetQueue = new Queue<NetworkMessage>();

        // Hàng đợi thông điệp mạng (yêu cầu danh sách file)
        private Queue<NetworkMessage> _listFileRequestQueue = new Queue<NetworkMessage>();

        // Bộ băm MD5
        private MD5 _hasher;

        // Khởi tạo trình gửi
        public UdpFileSender(string filesDirectory, int port)
        {
            try
            {
                FilesDirectory = filesDirectory;
                Port = port;
                _client = new UdpClient(Port, AddressFamily.InterNetwork); // IPv4
                _hasher = MD5.Create();
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi khi mở server: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Server đã mở tại cổng {0}.", Port);
            }
        }

        // Chuẩn bị trình gửi để truyền file
        public void Init()
        {
            // Lấy danh sách file có thể gửi
            List<string> files = new List<string>(Directory.EnumerateFiles(FilesDirectory));
            _transferableFiles = new HashSet<string>(files.Select(s => s.Substring(FilesDirectory.Length + 1)));

            // Kiểm tra xem có file nào để gửi không
            if (_transferableFiles.Count != 0)
            {
                Running = true;

                //// Tạo danh sách file và dung lượng
                //List<FileDetail> fileList = files.Select(f => new FileDetail(Path.GetFileName(f), new FileInfo(f).Length)).ToList();
                //
                //// Tạo gói tin chứa danh sách file
                //FileListPacket LIST = new FileListPacket(fileList);
                //
                //// Gửi gói tin
                //byte[] buffer = LIST.Payload;
                //IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
                //_client.Send(buffer, buffer.Length);
                //
                //// Xuất thông tin file ra màn hình
                //Console.WriteLine("Thông tin file đã gửi cho client:");
                //foreach (var file in fileList)
                //{
                //    Console.WriteLine("Tên file: {0}, Kích thước: {1} bytes", file.FileName, file.FileSize);
                //}
            }
            else
            {
                Console.WriteLine("Không có file nào để gửi.");
            }
        }

        // Dừng trình gửi
        public void Shutdown()
        {
            Running = false;
        }

        // Chạy trình gửi
        public void Run()
        {
            Running = true;
            // Trạng thái trình gửi
            SenderState state = SenderState.WaitingForFileRequest;
            Console.WriteLine("Đang chờ yêu cầu file...");

            // File yêu cầu
            string requestedFile = "";

            // IP nhận
            IPEndPoint receiver = null;

            // Hàm đặt lại trạng thái truyền
            Action ResetTransferState = new Action(() =>
            {
                state = SenderState.WaitingForFileRequest;
                requestedFile = "";
                receiver = null;
                _blocks.Clear();
            });

            while (Running)
            {
                // Kiểm tra thông điệp mạng
                _checkForNetworkMessages();

                // Nếu có thông điệp mạng thì lấy ra khỏi hàng đợi
                NetworkMessage nm = (_packetQueue.Count > 0) ? _packetQueue.Dequeue() : null;

                // Nếu có thông điệp BYE thì đặt lại trạng thái truyền
                bool isBye = (nm == null) ? false : nm.Packet.IsBye;
                if (isBye)
                {
                    ResetTransferState();
                    Console.WriteLine("Kết thúc truyền file.");
                }

                // 

                // Xử lý trạng thái truyền
                switch (state)
                {
                    // Chờ yêu cầu file
                    case SenderState.WaitingForFileRequest:
                        bool isRequestFile = (nm == null) ? false : nm.Packet.IsRequestFile;

                        // Nếu có yêu cầu file thì gửi ACK
                        if (isRequestFile)
                        {
                            RequestFilePacket REQF = new RequestFilePacket(nm.Packet); // Lấy thông tin yêu cầu file
                            AckPacket ACK = new AckPacket(); // Tạo ACK
                            requestedFile = REQF.Filename; // Lấy tên file

                            Console.WriteLine("{0} đã yêu cầu file {1}.", nm.Sender, requestedFile);

                            // Nếu có file thì gửi ACK
                            if (_transferableFiles.Contains(requestedFile))
                            {
                                receiver = nm.Sender; // Lưu lại IP nhận
                                ACK.Message = requestedFile; // Đặt thông điệp
                                state = SenderState.PreparingFileForTransfer; // Chuẩn bị file
                                Console.WriteLine("Gửi ACK cho {0}...", nm.Sender);
                            }
                            else
                            {
                                ResetTransferState();
                                Console.WriteLine("Không tìm thấy file {0}.", requestedFile);
                            }

                            // Chuyển ACK thành mảng byte và gửi
                            byte[] buffer = ACK.GetBytes(); 
                            _client.Send(buffer, buffer.Length, nm.Sender);
                        }
                        break;

                    // Chuẩn bị file để gửi
                    case SenderState.PreparingFileForTransfer:
                        byte[] checksum; // MD5 checksum
                        UInt32 fileSize; // Kích thước file

                        // Nếu chuẩn bị file thành công thì gửi thông tin file
                        if (_prepareFile(requestedFile, out checksum, out fileSize))
                        {
                            InfoPacket INFO = new InfoPacket(); // Tạo thông tin file
                            INFO.Checksum = checksum; // Đặt checksum
                            INFO.FileSize = fileSize;   // Đặt kích thước file
                            INFO.MaxBlockSize = MaxBlockSize; // Đặt kích thước khối tối đa
                            INFO.BlockCount = Convert.ToUInt32(_blocks.Count);  // Đặt số lượng khối

                            byte[] buffer = INFO.GetBytes();    // Chuyển thông tin file thành mảng byte
                            _client.Send(buffer, buffer.Length, receiver);  // Gửi thông tin file
                            Console.WriteLine("Gửi thông tin file cho {0}...", receiver);
                            Console.WriteLine("Đang đợi ACK...");
                            state = SenderState.WaitingForInfoACK;
                        }
                        else
                        {
                            Console.WriteLine("Lỗi khi chuẩn bị file {0} để gửi.", requestedFile);
                            ResetTransferState();
                        }
                        break;

                    // Chờ ACK từ người nhận
                    case SenderState.WaitingForInfoACK:
                        bool isAck = (nm == null) ? false : (nm.Packet.IsAck);
                        if (isAck)
                        {
                            // Nếu ACK là INFO thì bắt đầu truyền
                            AckPacket ACK = new AckPacket(nm.Packet);
                            if (ACK.Message == "INFO")
                            {
                                Console.WriteLine("Nhận ACK từ {0}. Bắt đầu truyền...", nm.Sender);
                                state = SenderState.Transfering; // Chuyển sang trạng thái truyền
                            }
                        }
                        break;

                    // Đang truyền
                    case SenderState.Transfering:
                        bool isRequestBlock = (nm == null) ? false : nm.Packet.IsRequestBlock;

                        // Nếu có yêu cầu khối thì gửi khối
                        if (isRequestBlock)
                        {
                            // Sử dụng luồng mới để gửi khối
                            Task.Run(() =>
                            {
                                // Lấy thông tin yêu cầu khối
                                RequestBlockPacket REQB = new RequestBlockPacket(nm.Packet);
                                Console.WriteLine("{0} đã yêu cầu khối #{1}.", nm.Sender, REQB.Number);
                                Block block = _blocks[REQB.Number]; // Lấy khối dữ liệu
                                SendPacket SEND = new SendPacket(); // Tạo thông điệp gửi dữ liệu
                                SEND.Block = block; // Đặt khối dữ liệu

                                byte[] buffer = SEND.GetBytes(); // Chuyển thông điệp thành mảng byte
                                _client.Send(buffer, buffer.Length, nm.Sender); // Gửi thông điệp

                                Console.WriteLine("Gửi khối #{0} cho {1} [{2} bytes].", REQB.Number, nm.Sender, block.Data.Length);
                            });
                        }
                        break;

                }

            Thread.Sleep(1);
            }

            // Gửi thông điệp BYE khi gửi hết khối
            if (receiver != null)
            {
                Packet BYE = new Packet(Packet.Bye);
                byte[] buffer = BYE.GetBytes();
                _client.Send(buffer, buffer.Length, receiver);
            }

            state = SenderState.NotRunning; // Đặt trạng thái không chạy
        }

        // Đóng trình gửi
        public void Close()
        {
            _client.Close();
        }

        // Kiểm tra kết nối tới máy chủ
        public bool Connect()
        {
            try
            {
                _client.Connect("localhost", Port);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        // Kiểm tra thông điệp mạng
        private void _checkForNetworkMessages()
        {
            //if (!Running)
            //    return;

            int bytesAvailable = _client.Available; // Số byte có sẵn

            // Nếu có ít nhất 4 byte thì lấy thông điệp mạng
            if (bytesAvailable >= 4)
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0); // Địa chỉ IP
                byte[] buffer = _client.Receive(ref ep);    // Nhận thông điệp mạng
                NetworkMessage nm = new NetworkMessage();   // Tạo thông điệp mạng
                nm.Sender = ep; // Đặt địa chỉ nguồn
                nm.Packet = new Packet(buffer); // Đặt thông điệp
                // Nếu thông điệp là yêu cầu danh sách file thì thêm vào hàng đợi
                if (nm.Packet.IsRequestList)
                    _listFileRequestQueue.Enqueue(nm);
                else
                    _packetQueue.Enqueue(nm);   // Thêm vào hàng đợi
                Console.WriteLine("Nhận thông điệp từ {0}: {1}", ep, nm.Packet.MessageTypeString);
            }

        }

        // Chuẩn bị file để gửi
        private bool _prepareFile(string requestedFile, out byte[] checksum, out UInt32 fileSize)
        {
            Console.WriteLine("Đang chuẩn bị file {0} để gửi...", requestedFile);
            bool good = false; // Kết quả
            fileSize = 0;

            try
            {
                // Đọc file
                byte[] fileBytes = File.ReadAllBytes(Path.Combine(FilesDirectory, requestedFile));

                // Tính MD5 checksum
                checksum = _hasher.ComputeHash(fileBytes);

                // Kích thước file
                fileSize = Convert.ToUInt32(fileBytes.Length);

                Console.WriteLine("Kích thước file {0}: {1} bytes.", requestedFile, fileSize);

                // Nén file
                Stopwatch timer = new Stopwatch();
                using (MemoryStream compressedStream = new MemoryStream())
                {
                    DeflateStream deflateStream = new DeflateStream(compressedStream, CompressionMode.Compress, true);
                    timer.Start();
                    deflateStream.Write(fileBytes, 0, fileBytes.Length); 
                    deflateStream.Close();
                    timer.Stop();

                    compressedStream.Position = 0;  // Vị trí byte khi nén
                    long compressedSize = compressedStream.Length; // Kích thước file nén
                    UInt32 id = 1; // Số thứ tự khối
                    while (compressedStream.Position < compressedSize)
                    {
                        // Số byte còn lại 
                        long numBytesLeft = compressedSize - compressedStream.Position;

                        // Số byte cần cấp phát (tối đa 8KB = Kích thước khối tối đa)
                        long allocationSize = (numBytesLeft > MaxBlockSize) ? MaxBlockSize : numBytesLeft;

                        // Đọc dữ liệu
                        byte[] data = new byte[allocationSize];
                        compressedStream.Read(data, 0, data.Length);

                        // Tạo khối dữ liệu
                        Block b = new Block(id++);
                        b.Data = data;
                        _blocks.Add(b.Number, b);
                    }

                    // Kiểm tra
                    Console.WriteLine("Đã nén file {0} thành {1} bytes trong {2:0.000}s.", requestedFile, compressedSize, timer.Elapsed.TotalSeconds);
                    Console.WriteLine("File đã được chia thành {0} khối.", _blocks.Count);
                    Console.WriteLine("Kích thước khối tối đa: {0} bytes.", MaxBlockSize);

                    good = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi khi chuẩn bị file {0} để gửi.", requestedFile);
                Console.WriteLine(e.Message);
                _blocks.Clear();
                checksum = null;
            }

            return good;
        }

        // Nhận yêu cầu danh sách file
        public void HandleListFileRequest()
        {
            while (true)
            {
                _checkForNetworkMessages();

                // Nếu có thông điệp mạng thì lấy ra khỏi hàng đợi
                NetworkMessage nm = (_listFileRequestQueue.Count > 0) ? _listFileRequestQueue.Dequeue() : null;

                bool isRequestList = (nm == null) ? false : nm.Packet.IsRequestList;
                if (isRequestList)
                {
                    Console.WriteLine("Nhận yêu cầu danh sách file từ {0}.", nm.Sender);
                    List<string> files = new List<string>(Directory.EnumerateFiles(FilesDirectory)); // Lấy danh sách file
                    List<FileDetail> fileList = files.Select(f => new FileDetail(Path.GetFileName(f), new FileInfo(f).Length)).ToList(); // Tạo danh sách file
                    FileListPacket LIST = new FileListPacket(fileList); // Tạo gói tin chứa danh sách file
                    byte[] buffer = LIST.GetBytes(); // Chuyển gói tin thành mảng byte
                    _client.Send(buffer, buffer.Length, nm.Sender); // Gửi gói tin
                    Console.WriteLine("Gửi danh sách file cho {0}.", nm.Sender);
                    // Thử in ra danh sách file
                    Console.WriteLine("Danh sách file:");
                    foreach (var file in fileList)
                    {
                        Console.WriteLine("Tên file: {0}, Kích thước: {1} bytes", file.FileName, file.FileSize);
                    }
                    Console.WriteLine(LIST.ToString()); // In ra thông tin gói tin
                    // Nếu có file thì running = true
                    if (fileList.Count > 0)
                        Running = true;
                    _transferableFiles = new HashSet<string>(fileList.Select(f => f.FileName));
                }
                Thread.Sleep(10);
            }
        }

        // Ping handler
        public void Ping(IPEndPoint sender)
        {
            Console.WriteLine("Ping từ {0}.", sender);

            // Gửi phản hồi Pong lại cho client
            string pongMessage = "Pong";
            byte[] buffer = Encoding.UTF8.GetBytes(pongMessage);
            _client.Send(buffer, buffer.Length, sender);

            Console.WriteLine("Đã gửi phản hồi Pong đến {0}.", sender);
        }

        // Bắt đầu server và lắng nghe yêu cầu từ client
        public void StartServer()
        {
            try
            {
                IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, Port);
                while (true)
                {
                    byte[] data = _client.Receive(ref clientEndPoint); // Nhận dữ liệu từ client
                    string message = Encoding.UTF8.GetString(data);
                    Console.WriteLine("Nhận thông điệp từ {0}: {1}", clientEndPoint, message);

                    // Nếu nhận được yêu cầu Ping, phản hồi với Pong
                    if (message == "Ping")
                    {
                        Console.WriteLine("Nhận yêu cầu Ping từ {0}.", clientEndPoint);
                        Ping(clientEndPoint); // Gửi phản hồi Pong
                    }

                    // Có thể thêm các yêu cầu khác ở đây (gửi file, nhận file, etc.)
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi xử lý yêu cầu: " + ex.Message);
            }
        }

        public void HandleMessage()
        {
            while (true)
            {
                _checkForNetworkMessages();
                Thread.Sleep(10);
            }
        }


    }
}
