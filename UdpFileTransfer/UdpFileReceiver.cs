using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Core;

namespace UdpFileTransfer
{
    public class UdpFileReceiver
    {
        #region Statics
        public static readonly int MD5ChecksumByteSize = 16; // Kích thước MD5 checksum (16 byte)
        #endregion // Statics

        enum ReceiverState
        {
            NotRunning, // Không chạy
            RequestingFile, // Đang yêu cầu file
            WaitingForRequestFileACK, // Đang chờ ACK từ yêu cầu file
            WaitingForInfo, // Đang chờ thông tin file
            PreparingForTransfer,   // Chuẩn bị cho việc truyền
            Transfering,    // Đang truyền
            TransferSuccessful, // Truyền thành công
        }

        // Thông điệp mạng
        private UdpClient _client;
        public readonly int Port;
        public readonly string Hostname; // Tên máy chủ
        private bool _shutdownRequested = false; // Yêu cầu dừng
        private bool _running = false; // Đang chạy

        // Dữ liệu
        private Dictionary<UInt32, Block> _blocksReceived = new Dictionary<UInt32, Block>(); // Các khối dữ liệu đã nhận
        private Queue<UInt32> _blockRequestQueue = new Queue<UInt32>(); // Các yêu cầu khối dữ liệu
        private Queue<NetworkMessage> _packetQueue = new Queue<NetworkMessage>(); // Các thông điệp mạng

        // MD5 hasher
        private MD5 _hasher;
        private long receiveBytes;
        private FileDetail fileDetail;
        // Khởi tạo kết nối UDP tới <hostname>:<port>
        public UdpFileReceiver(string hostname, int port)
        {
            Port = port;
            Hostname = hostname;

            // Khởi tạo client mặc định
            _client = new UdpClient(Hostname, Port);
            _hasher = MD5.Create();
        }

        // Yêu cầu dừng
        public void Shutdown()
        {
            _shutdownRequested = true;
        }

        // Nhận file
        public void GetFile(string filename, string filePath, int numThreads)
        {
            Console.WriteLine("Yêu cầu file: {0}", filename);
            long _fileSize = GetList().FirstOrDefault(m => m.fileName == filename).totalBytes;
            fileDetail = new FileDetail(filename, _fileSize, numThreads);
            // Thay đổi trạng thái thành RequestingFile
            ReceiverState state = ReceiverState.RequestingFile;

            byte[] checksum = null;
            UInt32 fileSize = 0;
            UInt32 numBlocks = 0;
            UInt32 totalRequestedBlocks = 0;
            Stopwatch transferTimer = new Stopwatch();

            // Reset trạng thái truyền
            Action ResetTransferState = new Action(() =>
            {
                state = ReceiverState.RequestingFile;
                checksum = null;
                fileSize = 0;
                numBlocks = 0;
                totalRequestedBlocks = 0;
                _blockRequestQueue.Clear();
                _blocksReceived.Clear();
                transferTimer.Reset();
            });

            _running = true;
            bool senderQuit = false;
            bool wasRunning = _running;

            bool isFirstTime = true;
            DateTime start = DateTime.Now;

            // Vòng lặp chính
            while (_running)
            {
                // Kiểm tra thông điệp mạng, nếu có thì lấy ra khỏi hàng đợi
                _checkForNetworkMessages();
                NetworkMessage nm = (_packetQueue.Count > 0) ? _packetQueue.Dequeue() : null;

                // Nếu thông điệp là BYE thì thoát
                bool isBye = (nm == null) ? false : nm.Packet.IsBye;
                if (isBye)
                    senderQuit = true;

                // Xử lý trạng thái
                switch (state)
                {
                    // Yêu cầu file
                    case ReceiverState.RequestingFile:
                        // Tạo yêu cầu file
                        RequestFilePacket REQF = new RequestFilePacket();
                        REQF.Filename = filename;

                        // Gửi yêu cầu (mảng byte)
                        byte[] buffer = REQF.GetBytes();
                        _client.Send(buffer, buffer.Length);

                        // Chuyển sang trạng thái chờ ACK
                        Console.WriteLine("Yêu cầu file thành công. Đang chờ ACK...");
                        state = ReceiverState.WaitingForRequestFileACK;
                        break;

                    // Chờ ACK từ yêu cầu file
                    case ReceiverState.WaitingForRequestFileACK:

                        // Kiểm tra ACK
                        bool isAck = (nm == null) ? false : (nm.Packet.IsAck);
                        if (isAck)
                        { 
                            AckPacket ACK = new AckPacket(nm.Packet);

                            // Kiểm tra đúng tên file
                            if (ACK.Message == filename)
                            {
                                // Chuyển sang trạng thái chờ INFO
                                state = ReceiverState.WaitingForInfo;
                                Console.WriteLine("Nhận ACK từ yêu cầu file thành công. Đang chờ thông tin file.");
                            }
                            else
                            {
                                Console.WriteLine("Yêu cầu file không thành công, đang thử lại...");
                                ResetTransferState();
                            }
                        }
                        break;

                    // Chờ thông tin file
                    case ReceiverState.WaitingForInfo:
                        // Kiểm tra INFO
                        bool isInfo = (nm == null) ? false : (nm.Packet.IsInfo);
                        if (isInfo)
                        {
                            // Lấy thông tin từ INFO
                            InfoPacket INFO = new InfoPacket(nm.Packet);
                            fileSize = INFO.FileSize;
                            checksum = INFO.Checksum;
                            numBlocks = INFO.BlockCount;

                            Console.WriteLine("Nhận gói INFO:");
                            Console.WriteLine("Kích thước khối tối đa: {0}", INFO.MaxBlockSize);
                            Console.WriteLine("Số khối: {0}", INFO.BlockCount);

                            // Gửi ACK
                            AckPacket ACK = new AckPacket();
                            ACK.Message = "INFO";
                            buffer = ACK.GetBytes();
                            _client.Send(buffer, buffer.Length);

                            // Chuyển sang trạng thái chuẩn bị truyền
                            state = ReceiverState.PreparingForTransfer;
                        }
                        break;

                    // Chuẩn bị truyền
                    case ReceiverState.PreparingForTransfer:
                        // Đưa các yêu cầu vào hàng đợi
                        for (UInt32 id = 1; id <= numBlocks; id++)
                            _blockRequestQueue.Enqueue(id);
                        totalRequestedBlocks += numBlocks;

                        // Bắt đầu truyền
                        Console.WriteLine("Bắt đầu truyền...");
                        transferTimer.Start(); // Đếm thời gian
                        state = ReceiverState.Transfering; // Chuyển sang trạng thái truyền
                        break;

                    // Truyền
                    case ReceiverState.Transfering:

                        if (isFirstTime)
                        {
                            isFirstTime = false;
                            start = DateTime.Now;
                        }
                        // Kiểm tra hàng đợi yêu cầu
                        if (_blockRequestQueue.Count > 0)
                        {
                            // Lấy số thứ tự khối
                            UInt32 id = _blockRequestQueue.Dequeue();

                            // Sử dụng Task để gửi yêu cầu khối
                            Task.Run(() =>
                            {
                                // Tạo yêu cầu khối
                                RequestBlockPacket REQB = new RequestBlockPacket();
                                REQB.Number = id;

                                // Gửi yêu cầu khối
                                buffer = REQB.GetBytes();
                                _client.Send(buffer, buffer.Length);

                                Console.WriteLine("Gửi yêu cầu khối #{0}", id);
                            });
                        }

                        // Kiểm tra khối dữ liệu đã nhận
                        bool isSend = (nm == null) ? false : (nm.Packet.IsSend);
                        if (isSend)
                        {
                            // Sử dụng Task để xử lý khối dữ liệu nhận được

                                // Lấy khối dữ liệu từ gói tin
                                SendPacket SEND = new SendPacket(nm.Packet); // Gói tin SEND
                                Block block = SEND.Block; // Khối dữ liệu
                                lock (_blocksReceived)
                                {
                                if (!_blocksReceived.ContainsKey(block.Number))
                                {
                                    _blocksReceived.Add(block.Number, block);
                                    receiveBytes += block.Data.Length;
                                }    

                                }


                            fileDetail.UpdateInfo((int)(receiveBytes * 100 / _fileSize), receiveBytes , 0, FileDownLoadStatus.Downloading);
                            Observer.Instance.Broadcast(EventId.OnProcessDownloadProgress, fileDetail);
                            Console.WriteLine("Nhận khối #{0} [{1} byte]", block.Number, block.Data.Length);
                        }

                        // Kiểm tra xem đã nhận đủ khối chưa
                        if ((_blockRequestQueue.Count == 0) && (_blocksReceived.Count != numBlocks))
                        {
                            // Yêu cầu lại các khối chưa nhận
                            for (UInt32 id = 1; id <= numBlocks; id++)
                            {
                                // Nếu chưa nhận khối này thì yêu cầu
                                if (!_blocksReceived.ContainsKey(id) && !_blockRequestQueue.Contains(id))
                                {
                                    _blockRequestQueue.Enqueue(id);
                                    totalRequestedBlocks++;
                                }
                            }
                        }

                        // Đủ khối thì chuyển sang trạng thái TransferSuccessful
                        if (_blocksReceived.Count == numBlocks)
                        {
                            double downTime = (DateTime.Now - start).TotalSeconds;
                            state = ReceiverState.TransferSuccessful;
                        }

                        break;

                    // Truyền thành công
                    case ReceiverState.TransferSuccessful:
                        transferTimer.Stop();

                        // Gửi BYE
                        Packet BYE = new Packet(Packet.Bye);
                        buffer = BYE.GetBytes();
                        _client.Send(buffer, buffer.Length);

                        Console.WriteLine("Truyền file thành công");
                        Console.WriteLine("Thời gian: {0:0.000}s", transferTimer.Elapsed.TotalSeconds);
                        Console.WriteLine("Giải nén các khối...");

                        // Lưu file
                        if (_saveBlocksToFile(filename, checksum, fileSize, filePath))
                            Console.WriteLine("Lưu file {0} thành công.", filename);
                        else
                            Console.WriteLine("Có lỗi khi lưu file {0}.", filename);

                        // Kết thúc
                        _running = false;
                        break;

                }

                Thread.Sleep(1);


                // Kiểm tra yêu cầu dừng
                _running &= !_shutdownRequested;
                _running &= !senderQuit;
            }

            // Nếu yêu cầu dừng thì gửi BYE
            if (_shutdownRequested && wasRunning)
            {
                Console.WriteLine("Yêu cầu dừng truyền file...");

                // Gửi BYE
                Packet BYE = new Packet(Packet.Bye);
                byte[] buffer = BYE.GetBytes();
                _client.Send(buffer, buffer.Length);
            }

            // Nếu người gửi dừng thì thông báo
            if (senderQuit && wasRunning)
                Console.WriteLine("Người gửi dừng truyền file.");

            ResetTransferState();   // Reset trạng thái truyền
            _shutdownRequested = false; // Reset yêu cầu dừng
        }

        // Đóng kết nối
        public void Close()
        {
            _client.Close();
        }

        // Kiểm tra thông điệp mạng
        private void _checkForNetworkMessages()
        {
            //if (!_running)
            //    return;
              
            // Kiểm tra xem có thông điệp mạng nào không
            int bytesAvailable = _client.Available; // Số byte có sẵn
            if (bytesAvailable >= 4)
            {
                // Nhận thông điệp mạng
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
                byte[] buffer = _client.Receive(ref ep);
                Packet p = new Packet(buffer); // Nhận gói tin

                // Thêm thông điệp mạng vào hàng đợi
                NetworkMessage nm = new NetworkMessage();
                nm.Sender = ep;
                nm.Packet = p;
                _packetQueue.Enqueue(nm);
            }


        }

        // Lưu các khối dữ liệu vào file
        private bool _saveBlocksToFile(string filename, byte[] networkChecksum, UInt32 fileSize, string filePath)
        {
            bool good = false;

            try
            {
                // Tính kích thước dữ liệu nén
                int compressedByteSize = 0;
                foreach (Block block in _blocksReceived.Values)
                    compressedByteSize += block.Data.Length;
                byte[] compressedBytes = new byte[compressedByteSize];

                // Gộp các khối dữ liệu (nén) vào mảng byte để giải nén
                int cursor = 0; // Vị trí
                for (UInt32 id = 1; id <= _blocksReceived.Keys.Count; id++)
                {
                    Block block = _blocksReceived[id]; // Lấy khối dữ liệu
                    block.Data.CopyTo(compressedBytes, cursor); // Gộp vào mảng byte
                    cursor += Convert.ToInt32(block.Data.Length);   // Tăng vị trí
                }

                // Giải nén
                using (MemoryStream uncompressedStream = new MemoryStream()) // Luồng dữ liệu đã giải nén
                using (MemoryStream compressedStream = new MemoryStream(compressedBytes)) // Luồng dữ liệu nén
                using (DeflateStream deflateStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
                {
                    deflateStream.CopyTo(uncompressedStream); // Giải nén

                    // Kiểm tra checksum
                    uncompressedStream.Position = 0;
                    byte[] checksum = _hasher.ComputeHash(uncompressedStream); // Tính checksum
                    if (!Enumerable.SequenceEqual(networkChecksum, checksum)) // So sánh checksum
                        throw new Exception("Checksum của các khối dữ liệu giải nén không khớp với checksum của gói INFO.");

                    // Ghi vào file (lưu file)
                    uncompressedStream.Position = 0;
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                        uncompressedStream.CopyTo(fileStream);
                }

                good = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Không thể lưu các khối dữ liệu vào file \"{0}\"", filename);
                Console.WriteLine(e.Message);
            }

            return good;
        }

        // Nhận danh sách file
        public List<FileDetail> GetList()
        {
            bool OK = false; // Kiểm tra nhận file thành công hay chưa
            List<FileDetail> files = new List<FileDetail>();
            const int timeout = 1000; // 1 giây

            while (!OK)
            {
                // Gửi yêu cầu danh sách file
                Packet REQL = new Packet(Packet.RequestList);
                byte[] buffer = REQL.GetBytes();
                _client.Send(buffer, buffer.Length);
                Console.WriteLine("Yêu cầu danh sách file...");

                NetworkMessage nm = null;
                Stopwatch stopwatch = Stopwatch.StartNew();

                while (stopwatch.ElapsedMilliseconds < timeout)
                {
                    // Nhận danh sách file
                    _checkForNetworkMessages();
                    if (_packetQueue.Count > 0)
                    {
                        nm = _packetQueue.Dequeue();
                        break;
                    }
                }

                // Kiểm tra danh sách file
                bool isListFile = (nm == null) ? false : (nm.Packet.IsListFile);
                if (isListFile)
                {
                    Console.WriteLine("Nhận danh sách file...");
                    FileListPacket LIST = new FileListPacket(nm.Packet);
                    files = LIST.FileList;
                    OK = true;
                }
                else
                {
                    Console.WriteLine("Không nhận được danh sách file. Thử lại...");
                }
            }

            return files;
        }


        // Ping server
        public bool Ping(string serverAddress, int port)
        {
            try
            {
                // Gửi yêu cầu Ping đến server
                UdpClient pingClient = new UdpClient();
                IPEndPoint serverEndpoint = new IPEndPoint(IPAddress.Parse(serverAddress), port);

                string pingMessage = "Ping";
                byte[] buffer = Encoding.UTF8.GetBytes(pingMessage);

                pingClient.Send(buffer, buffer.Length, serverEndpoint);
                Console.WriteLine("Đã gửi yêu cầu Ping đến {0}:{1}.", serverAddress, port);

                // Đợi phản hồi Pong từ server
                pingClient.Client.ReceiveTimeout = 3000; // Đặt thời gian chờ tối đa 3 giây
                byte[] response = pingClient.Receive(ref serverEndpoint); // Nhận phản hồi

                string responseMessage = Encoding.UTF8.GetString(response);
                if (responseMessage == "Pong")
                {
                    Console.WriteLine("Nhận phản hồi Pong từ server.");
                    return true; // Kết nối thành công
                }
                else
                {
                    Console.WriteLine("Phản hồi không hợp lệ từ server.");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi khi ping server: " + e.Message);
                return false; // Kết nối không thành công
            }
        }

        public bool Connect()
        {
            try
            {
                _client.Connect("localhost", Port);  // hoặc thay bằng địa chỉ IP cụ thể
                Console.WriteLine("Kết nối thành công tới server.");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi khi kết nối: " + e.Message);
                return false;
            }
        }


    }
}