using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
        private Thread updateThread;
        private List<Connection> connections = new List<Connection>();
        public int maxPlayerNumber = 2;
        public int playerCount
        {
            get { return connections.Count; }
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
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("Sever Started");
                Console.ResetColor();
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
                throw;
            }

            this.serverThread = new Thread(this.ServerThreadProc);
            this.serverThread.IsBackground = true;

            this.serverThread.Start();

            this.updateThread = new Thread(this.ClientUpdateProc);
            this.updateThread.IsBackground = true;

            this.updateThread.Start();
        }

        void ServerThreadProc()
        {
            while (!terminateServer)
            {
                while (this.tcpListener.Pending())
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Client wants to Connect");
                    Console.ResetColor();

                    lock (this.connections)
                    {
                        if (this.playerCount < playerCount)
                        {
                            Connection tempConnection = new Connection(tcpClient);

                            this.connections.Add(tempConnection);
                        }
                    }
                }
            }

            Thread.Sleep(50);
        }

        void ClientUpdateProc()
        {
            while (!terminateServer)
            {
                foreach (var connection in connections)
                {
                    connection.Update();
                }

                Thread.Sleep(17);
            }
        }
    }
}
