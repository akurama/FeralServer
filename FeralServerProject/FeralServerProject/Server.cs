using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace FeralServerProject
{
    public class Server
    {
        private TcpListener tcpListener;
        private int SERVERPORT = 2018;
        private bool terminateServer = false;
        private Thread serverThread;
        private List<TcpClient> tcpClients = new List<TcpClient>();
        public int maxPlayerNumber = 2;
        public int playerCount
        {
            get { return tcpClients.Count; }
        }

        public Server()
        {
            StartServer();
        }

        void StartServer()
        {
            Console.WriteLine("Es ist Matchmaking 1 gebt den Server doch erstmal nen Like. Schreibs in die Kommentare findest du geil dann gibts bald Version 2");

            this.terminateServer = false;

            this.tcpListener = new TcpListener(SERVERPORT);

            try
            {
                tcpListener.Start();
                Console.WriteLine("Sever Started");
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
                throw;
            }

            this.serverThread = new Thread(this.ServerThreadProc);
            this.serverThread.IsBackground = true;

            this.serverThread.Start();
        }

        void ServerThreadProc()
        {
            while (!terminateServer)
            {
                while (this.tcpListener.Pending())
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    Console.WriteLine("Client wants to Connect");
                }
            }
        }
    }
}
