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
        private Thread updateThread;
        private Thread headbeatThread;
        private List<Connection> connections = new List<Connection>();
        private List<Connection> disconnectedConnections = new List<Connection>();
        public static bool listLocked = false;
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
                        ConsoleLogs.ConsoleLog(ConsoleColor.Gray, "Current Connected Clients: " + playerCount);
                        if (this.playerCount < maxPlayerNumber)
                        {
                            ConsoleLogs.ConsoleLog(ConsoleColor.Green, "Client can connect");
                            Connection tempConnection = new Connection(tcpClient, this.playerCount);
                            tempConnection.MessageRecieved += OnMessageRecieved;

                            this.connections.Add(tempConnection);
                        }
                        else
                        {
                            ConsoleLogs.ConsoleLog(ConsoleColor.Red,
                                "Client cannot connect, because the Lobby is full");
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
                for (int i = 0; i < connections.Count; i++)
                {
                    connections[i].Update();
                }

                Thread.Sleep(17);
            }
        }

        void ClientHeadBeat()
        {
            while (!terminateServer)
            {
                var m = new HeartbeatMessage()
                {
                    heartbeat = 2
                };
                
                for (int i = 0; i < connections.Count; i++)
                {
                    ConsoleLogs.ConsoleLog(ConsoleColor.Gray, "Heartbeat");
                    try
                    {
                        connections[i].Send(m);
                    }
                    catch (Exception e)
                    {
                        ConsoleLogs.ConsoleLog(ConsoleColor.Red, e.ToString());
                        disconnectedConnections.Add(connections[i]);
                        connections[i].MessageRecieved -= OnMessageRecieved;
                    }
                }

                HelperFunctions.RemoveDisconnectedClients(disconnectedConnections, connections);

                Thread.Sleep(3000);
            }
        }

        void OnMessageRecieved(MessageBase message, Connection senderConnection)
        {
            ConsoleLogs.ConsoleLog(ConsoleColor.Blue, "Sie haben post");

            if (message is DisconnectMessage)
            {
                //TODO: Remove client from List of Connected Clients
                disconnectedConnections.Add(senderConnection);
                senderConnection.MessageRecieved -= OnMessageRecieved;
            }

            if (message is ConnectMessage)
            {
                var m = (ConnectMessage) message;
                if (m.clientID == 0)
                {
                    //TODO Assign ClientID from Server
                }

                message = m;
            }
              
            HelperFunctions.RemoveDisconnectedClients(disconnectedConnections, connections);
            
            for (int i = 0; i < connections.Count; i++)
            {
                try
                {
                    ConsoleLogs.ConsoleLog(ConsoleColor.Gray, "Sending Awnser");
                    connections[i].Send(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            LogMessage(message);
        }

        void LogMessage(MessageBase message)
        {
            if (message is ConnectMessage)
            {
                var m = (ConnectMessage) message;
                ConsoleLogs.ConsoleLog(ConsoleColor.Green, m.senderName + " has connected");
            }
            else if (message is DisconnectMessage)
            {
                var m = (DisconnectMessage) message;
            }
            else if (message is ReadyMessage)
            {
                var m = (ReadyMessage) message;
            }
            else if (message is StartGameMessage)
            {
                var m = (StartGameMessage) message;
            }
            else if (message is StopGameMessage)
            {
                var m = (StopGameMessage) message;
            }
            else if (message is ChatMessage)
            {
                var m = (ChatMessage) message;
                ConsoleLogs.ConsoleLog(ConsoleColor.Blue, m.SenderName + ": " + m.messageText);
            }
            else if (message is HeartbeatMessage)
            {
                //Wird nie vom Server zurück gegeben
                var m = (HeartbeatMessage) message;
            }
            else if (message is GameStateMessage)
            {
                var m = (GameStateMessage) message;
            }
            else if (message is GameInputMessage)
            {
                var m = (GameInputMessage) message;
                ConsoleLogs.ConsoleLog(ConsoleColor.DarkCyan, "From Cell: " + m.fromCellIndex + " To Cell: " + m.toCellIndex + " Interaction Type: " + m.interactionType);
            }
            else if (message is EndTurnMessage)
            {
                var m = (EndTurnMessage) message;
            }   
            else if (message is GameSettingsMessage)
            {
                var m = (GameInputMessage) message;
            }
        }
    }
}