using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace test
{
    public class UdpStringSender
    {
        private UdpClient _client;
        public readonly int Port;

        public UdpStringSender(int port)
        {
            Port = port;
            _client = new UdpClient();
        }

        public void Send(string message, string hostname, int receiverPort)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            _client.Send(buffer, buffer.Length, hostname, receiverPort);
            Console.WriteLine("Sent message: " + message);
        }
    }
 }