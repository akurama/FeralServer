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
        private Thread updateThread;
        private Thread headbeatThread;
        private List<Connection> connections = new List<Connection>();
        private List<Connection> disconnectedConnections = new List<Connection>();
        private int numOfConnectedClients = 0;
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
                    numOfConnectedClients++;

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
            if (message is DisconnectMessage)
            {
                disconnectedConnections.Add(senderConnection);
                senderConnection.MessageRecieved -= OnMessageRecieved;
                HelperFunctions.RemoveDisconnectedClients(disconnectedConnections, connections);

                for (int i = 0; i < connections.Count; i++)
                {
                    ClientInformationMessage m = new ClientInformationMessage(connections[i].ClientId, i, 0);
                    connections[i].PlayerID = i;
                    connections[i].Send(m);
                }
            }

            if (message is ConnectMessage)
            {
                string clientID = (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds.ToString() + "feral" + numOfConnectedClients;
                int playerID = connections.Count - 1;
                
                ClientInformationMessage m = new ClientInformationMessage(clientID, playerID, 0);
                senderConnection.Send(m);
                senderConnection.ClientId = clientID;
                senderConnection.PlayerID = playerID;
                for (int i = 0; i < connections.Count; i++)
                {
                    try
                    {
                        m.informationType = 1;
                        if(connections[i] != senderConnection)
                            connections[i].Send(m);
                        ClientInformationMessage m1 = new ClientInformationMessage(connections[i].ClientId, connections[i].PlayerID, 1);
                        senderConnection.Send(m1);
                    }
                    catch (Exception e)
                    {
                        ConsoleLogs.ConsoleLog(ConsoleColor.Red, e.ToString());
                        disconnectedConnections.Add(connections[i]);
                        connections[i].MessageRecieved -= OnMessageRecieved;
                    }
                }
            }
              
            
            
            for (int i = 0; i < connections.Count; i++)
            {
                try
                {
                    connections[i].Send(message);
                }
                catch (Exception e)
                {
                    ConsoleLogs.ConsoleLog(ConsoleColor.Red, e.ToString());
                    disconnectedConnections.Add(connections[i]);
                    connections[i].MessageRecieved -= OnMessageRecieved;
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
                ConsoleLogs.ConsoleLog(ConsoleColor.DarkGreen, "Client with the ID" + m.clientID + " has disconnected.");
            }
            else if (message is ReadyMessage)
            {
                var m = (ReadyMessage) message;
                eReadyState readyState = (eReadyState) m.readyState;
                ConsoleLogs.ConsoleLog(ConsoleColor.Magenta, "Player with the client id " + m.clientID + 
                                                             (readyState == eReadyState.Ready ? " is Ready" : " is not Ready"));
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
                ConsoleLogs.ConsoleLog(ConsoleColor.DarkYellow, "Player " + m.playerIDold + " finished Turn " + m.turnIDold + " now its Player " + m.playerIDnew + " turn");
            }   
            else if (message is GameSettingsMessage)
            {
                var m = (GameInputMessage) message;
            }
        }
    }
}