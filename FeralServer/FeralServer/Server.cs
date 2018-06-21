using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using FeralServerProject.Collections;
using FeralServerProject.Extensions;
using FeralServerProject.Messages;
using Microsoft.Win32;

namespace FeralServerProject
{
    public class Server
    {
        private TcpListener tcpListener;
        private int SERVERPORT = 2018;
        
        private bool terminateServer = false;
        private Thread serverThread;
        
        public int numOfConnectedClients = 0;
        public int numOfCreatedRooms = 0;
        public static bool listLocked = false;
        public int maxPlayerNumber = 2;

        public List<Room> rooms = new List<Room>();

        public static Server instance;

        public Server()
        {
            if (instance == null)
            {
                instance = this;
            }
            
            StartServer();
        }

        void StartServer()
        {
            ConsoleLogs.ConsoleLog(ConsoleColor.White,
                "Es ist Matchmaking 1 gebt den Server doch erstmal nen Like. Schreibs in die Kommentare findest du geil dann gibts bald Version 2");

            this.terminateServer = false;
            this.tcpListener = new TcpListener(IPAddress.Any, SERVERPORT);

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
            
            rooms.Add(new Room("Lobby", Int32.MaxValue));
        }

        void ServerThreadProc()
        {
            while (!terminateServer)
            {
                while (this.tcpListener.Pending())
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    numOfConnectedClients++;

                    lock (this.rooms[0].connections)
                    {   
                        Connection tempConnection = new Connection(tcpClient, this.rooms[0].playerCount);
                        rooms[0].AddClient(tempConnection);
                    }
                }
            }
            
            Thread.Sleep(50);
        }
    }
}