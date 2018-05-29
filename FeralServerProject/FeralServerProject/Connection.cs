using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using FeralServerProject.Messages;

namespace FeralServerProject
{
    public class Connection
    {
        private TcpClient tcpClient;
        private NetworkStream networktStream;
        private int PlayerID;
        private byte[] receiveBuffer = new byte[1024];

        public Connection(TcpClient client)
        {
            this.tcpClient = client;
            this.networktStream = tcpClient.GetStream();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("New Client Created");
            Console.ResetColor();

            
        }

        private void MessageProtocoal_MessaceCoplete(byte[] obj)
        {
            
        }

        public void Send(MessageBase m)
        {

        }

        public void Update()
        {
            while (this.networktStream.DataAvailable)
            {
                Console.WriteLine("Data Availeble");
                this.networktStream.Read(receiveBuffer, 0,  receiveBuffer.Length);
            }
        }
    }
}
