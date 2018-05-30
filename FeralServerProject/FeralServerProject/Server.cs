using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using FeralServerProject.Extensions;

namespace FeralServerProject
{
    public class Server
    {
        private TcpListener tcpListener;
        private int SERVERPORT = 2018;
        private bool terminateServer = false;
        private Thread serverThread;
        private Thread updateThread;
        private Thread headbeatThread;
        private List<Connection> connections = new List<Connection>();
        private List<Connection> disconnectedConnections = new List<Connection>();
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
            ConsoleLogs.ConsoleLog(ConsoleColor.White, "Es ist Matchmaking 1 gebt den Server doch erstmal nen Like. Schreibs in die Kommentare findest du geil dann gibts bald Version 2");

            this.terminateServer = false;

            this.tcpListener = new TcpListener(SERVERPORT);

            try
            {
                tcpListener.Start();
                ConsoleLogs.ConsoleLog(ConsoleColor.Magenta, "Server Started");
            }
            catch (SocketException e)
            {
                ConsoleLogs.ConsoleLog(ConsoleColor.Red, e.ToString());
                throw;
            }

            this.serverThread = new Thread(this.ServerThreadProc);
            this.serverThread.IsBackground = true;

            this.serverThread.Start();

            this.updateThread = new Thread(this.ClientUpdateProc);
            this.updateThread.IsBackground = true;

            this.updateThread.Start();

            this.headbeatThread = new Thread(this.ClientHeadBeat);
            this.headbeatThread.IsBackground = true;
            this.headbeatThread.Start();
        }

        void ServerThreadProc()
        {
            while (!terminateServer)
            {
                while (this.tcpListener.Pending())
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    ConsoleLogs.ConsoleLog(ConsoleColor.Green, "Client wants to connect");

                    lock (this.connections)
                    {
                        ConsoleLogs.ConsoleLog(ConsoleColor.Gray, "Currend Connected Clients: " + playerCount);
                        if (this.playerCount < maxPlayerNumber)
                        {
                            ConsoleLogs.ConsoleLog(ConsoleColor.Green, "Client can connect");
                            Connection tempConnection = new Connection(tcpClient);

                            this.connections.Add(tempConnection);
                        }
                        else
                        {
                            ConsoleLogs.ConsoleLog(ConsoleColor.Red, "Client cannot connect, because the Lobby is full");
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

        void ClientHeadBeat()
        {
            while (!terminateServer)
            {
                MemoryStream memoryStream = new MemoryStream();
                BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

                binaryWriter.Write("Hearbeat");
                
                foreach (var connection in connections)
                {
                    ConsoleLogs.ConsoleLog(ConsoleColor.White, "Sending Heartbeat");
                    try
                    {
                        connection.NetworkStream.Write(memoryStream.ToArray(), 0, memoryStream.ToArray().Length);
                    }
                    catch (Exception e)
                    {
                        ConsoleLogs.ConsoleLog(ConsoleColor.Red, e.ToString());
                        disconnectedConnections.Add(connection);
                    }
                }

                HelperFunctions.RemoveDisconnectedClients(disconnectedConnections, connections);

                Thread.Sleep(1000);
            }
        }
    }
}
