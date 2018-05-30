using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using FeralServerProject.Extensions;
using FeralServerProject.Messages;

namespace FeralServerProject
{
    public class Connection
    {
        private TcpClient tcpClient;
        public TcpClient TcpClient
        {
            get { return tcpClient; }
        }

        private NetworkStream networktStream;
        public NetworkStream NetworkStream
        {
            get { return networktStream; }
        }

        private int PlayerID;
        private byte[] receiveBuffer = new byte[1024];
        private MessageProtocoll messageProtocoll;

        public Connection(TcpClient client)
        {
            this.tcpClient = client;
            this.networktStream = tcpClient.GetStream();

            ConsoleLogs.ConsoleLog(ConsoleColor.Green, "New Client Created");
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
                ConsoleLogs.ConsoleLog(ConsoleColor.Gray, "New data availeble");
                int bytesData = this.networktStream.Read(receiveBuffer, 0, receiveBuffer.Length);
                this.messageProtocoll.ReceiveData(this.receiveBuffer, 0, bytesData);
            }
        }
    }
}
