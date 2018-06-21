﻿using System;
using System.Collections.Generic;
using System.Threading;
using FeralServerProject.Extensions;
using FeralServerProject.Messages;
using FeralServerProject.Collections;

namespace FeralServerProject
{
    public class Room
    {
        public Thread updateThread;
        public Thread heartbeatThread;
        
        public List<Connection> connections = new List<Connection>();
        public List<Connection> disconnectedConnections = new List<Connection>();

        public int maxPlayerNumber = Int32.MaxValue;
        public string roomName = "";
        public string roomID = "";
        public string roomPasswort = "";

        private bool terminateRoom = false;
        
        private HeartbeatMessage heartbeat = new HeartbeatMessage()
        {
            heartbeat = 2
        };
        
        public int playerCount
        {
            get { return connections.Count; }
        }
            
        public Room(string roomName, int maxPlayerNumber)
        {
            this.roomName = roomName;
            this.maxPlayerNumber = maxPlayerNumber;
            this.roomID = (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds + "feralroom" +
                          Server.instance.numOfCreatedRooms;
            Server.instance.numOfCreatedRooms++;
            StartRoom();
        }

        public Room(string roomName, int maxPlayerNumber, string roomPasswort)
        {
            this.roomName = roomName;
            this.maxPlayerNumber = maxPlayerNumber;
            this.roomID = (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds + "feralroom" +
                          Server.instance.numOfCreatedRooms;
            this.roomPasswort = roomPasswort;
            StartRoom();
        }

        public void StartRoom()
        {
            //Starts the Update Tick of a room
            this.updateThread = new Thread(this.RoomUpdateProc);
            this.updateThread.IsBackground = true;
            this.updateThread.Start();
            
            //Starts the Heartbeat
            this.heartbeatThread = new Thread(this.RoomHeartbeatProc);
            this.heartbeatThread.IsBackground = true;
            this.heartbeatThread.Start();
        }

        void RoomUpdateProc()
        {
            while (!terminateRoom)
            {
                for (int i = 0; i < connections.Count; i++)
                {
                    connections[i].Update();
                }
                
                Thread.Sleep(17);
            }
        }

        void RoomHeartbeatProc()
        {
            while (!terminateRoom)
            {
                for (int i = 0; i < connections.Count; i++)
                {
                    try
                    {
                        connections[i].Send(heartbeat);
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
                    ClientInformationMessage m = new ClientInformationMessage(connections[i].ClientId, connections[i].Username, i, 0);
                    connections[i].PlayerID = i;
                    connections[i].Send(m);
                }
            }

            if (message is ConnectMessage)
            {
                string clientID = (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds.ToString() + "feral" + Server.instance.numOfConnectedClients;
                int playerID = connections.Count - 1;
                
                ClientInformationMessage m = new ClientInformationMessage(clientID, ((ConnectMessage)message).userName, playerID, 0);
                senderConnection.Send(m);
                senderConnection.Username = ((ConnectMessage) message).userName;
                senderConnection.ClientId = clientID;
                senderConnection.PlayerID = playerID;
                for (int i = 0; i < connections.Count; i++)
                {
                    try
                    {
                        m.informationType = 1;
                        if(connections[i] != senderConnection)
                            connections[i].Send(m);
                        ClientInformationMessage m1 = new ClientInformationMessage(connections[i].ClientId, connections[i].Username, connections[i].PlayerID, 1);
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
            
            ConsoleLogs.LogMessage(message);
        }

        public void LeaveRoom(Connection connection)
        {
            connection.MessageRecieved -= OnMessageRecieved;
            connections.Remove(connection);
            
            Server.instance.rooms[0].AddClient(connection);
        }

        public void AddClient(Connection connection)
        {
            connection.MessageRecieved += OnMessageRecieved;
            this.connections.Add(connection);
        }
    }   
}