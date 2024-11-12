using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public class UdpStringReceiver
    {
        private UdpClient _client;
        public readonly int Port;

        public UdpStringReceiver(int port)
        {
            Port = port;
            _client = new UdpClient(port);
        }

        public void StartListening()
        {
            Console.WriteLine("Receiver is listening...");
            while (true)
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, Port);
                byte[] buffer = _client.Receive(ref ep);
                string message = Encoding.UTF8.GetString(buffer);
                Console.WriteLine("Received message: " + message);
            }
        }
    }

}
