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
        public event Action<MessageBase, Connection> MessageRecieved; 

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

        private byte[] receiveBuffer = new byte[1024];
        private MessageProtocoll messageProtocoll = new MessageProtocoll();

        private int playerID;
        public int PlayerID
        {
            get { return playerID; }
            set { playerID = value; }
        }

        private string clientID;
        public string ClientId
        {
            get { return clientID; }
            set { clientID = value; }
        }

        public int receivedMessages;

        //Currentstep: 0
        //Im Server Liste 

        public Connection(TcpClient client, int playerID)
        {
            this.tcpClient = client;
            this.networktStream = tcpClient.GetStream();
            this.playerID = playerID;

            this.messageProtocoll.MessageComplete += MessageProtocoal_MessaceCoplete;

            ConsoleLogs.ConsoleLog(ConsoleColor.Green, "New Client Created");
        }

        private void MessageProtocoal_MessaceCoplete(byte[] obj)
        {
            var message = MessageBase.FromByteArray(obj);
            if (this.MessageRecieved != null)
            {
                this.MessageRecieved(message, this);
            }
        }

        public void Send(MessageBase m)
        {
            var b = m.ToByteArray();
            this.networktStream.Write(b, 0, b.Length);
        }

        public void Update()
        {
            while (this.networktStream.DataAvailable)
            {
                ConsoleLogs.ConsoleLog(ConsoleColor.Gray, "New data available");
                int bytesData = this.networktStream.Read(receiveBuffer, 0, receiveBuffer.Length);
                this.messageProtocoll.ReceiveData(this.receiveBuffer, 0, bytesData);
            }
        }
    }
}
