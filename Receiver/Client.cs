using Core;
using System;
using System.Collections.Generic;
using UdpFileTransfer;

namespace Receiver
{
    public class Client
    {
        private string ip;
        private int port;
        private string filePath;
        private string fileName;
        private int numThreads;
        public static UdpFileReceiver fileReceiver;
        public Client(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            fileReceiver = new UdpFileReceiver(ip, port);
        }
        public Client(string ip, int port, string fileName, string filePath, int numThreads)
        {
            this.ip = ip;
            this.port = port;
            this.fileName = fileName;
            this.filePath = filePath;
            this.numThreads = numThreads;
            fileReceiver = new UdpFileReceiver(ip, port);
        }


        public List<FileDetail> GetFileList()
        {
            List<FileDetail> files = fileReceiver.GetList();
            return files;
        }
        public void StartDownload()
        {
            fileReceiver.GetFile(fileName, filePath, numThreads);
        }
        static void Main(string[] args)
        {
           
        }
    }
}
