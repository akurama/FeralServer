using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public string roomHost = "";

        public bool isLobby = false;

        private bool terminateRoom = false;

        private HeartbeatMessage heartbeat = new HeartbeatMessage()
        {
            heartbeat = 2
        };

        public int playerCount
        {
            get { return connections.Count; }
        }

        public Room(string roomName, string roomHost, int maxPlayerNumber)
        {
            this.roomName = roomName;
            this.roomHost = roomHost;
            this.maxPlayerNumber = maxPlayerNumber;
            this.roomID = (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds + "feralroom" +
                          Server.instance.numOfCreatedRooms;
            Server.instance.numOfCreatedRooms++;
            StartRoom();
        }

        public Room(string roomName, int maxPlayerNumber, bool isLobby)
        {
            this.roomName = roomName;
            this.maxPlayerNumber = maxPlayerNumber;
            this.roomID = (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds + "feralroom" +
                          Server.instance.numOfCreatedRooms;
            this.isLobby = isLobby;
            Server.instance.numOfCreatedRooms++;
            StartRoom();
        }

        public Room(string roomName, string roomHost, int maxPlayerNumber, string roomPasswort)
        {
            this.roomName = roomName;
            this.maxPlayerNumber = maxPlayerNumber;
            this.roomHost = roomHost;
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
                CheckRoomState();

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
                senderConnection.Username = ((ConnectMessage)message).userName;
                senderConnection.ClientId = clientID;
                senderConnection.PlayerID = playerID;

                if (this == Server.instance.rooms[0])
                {

                }
                else
                {
                    for (int i = 0; i < connections.Count; i++)
                    {
                        try
                        {
                            m.informationType = 1;
                            if (connections[i] != senderConnection)
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
            }

            if (message is RoomCreationMessage)
            {
                CreateRoom((RoomCreationMessage)message, senderConnection);
            }

            if (message is RoomJoinMessage)
            {
                var m = (RoomJoinMessage) message;
                Room target = FindRoom(m.roomID);

                Console.WriteLine(target.isLobby);
                Console.WriteLine(target.playerCount);
                Console.WriteLine(target.maxPlayerNumber);

                if (!target.isLobby && target.playerCount < target.maxPlayerNumber)
                {
                    LeaveRoom(senderConnection, target);
                    ((RoomJoinMessage)message).result = 0;
                    senderConnection.Send(message);
                }
                else
                {
                    ((RoomJoinMessage)message).result = 1;
                    senderConnection.Send(message);
                }
            }

            if (message is PlayerRenameMessage)
            {
                senderConnection.Username = ((PlayerRenameMessage) message).newName;
                if (senderConnection == connections[0])
                {
                    roomHost = ((PlayerRenameMessage) message).newName;
                }
            }

            if (message is RoomListUpdateMessage)
            {
                SendRoomList(senderConnection);
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
            CheckRoomState();
            Server.instance.rooms[0].AddClient(connection);
        }

        public void LeaveRoom(Connection connection, Room newRoom)
        {
            connection.MessageRecieved -= OnMessageRecieved;
            connections.Remove(connection);
            CheckRoomState();
            JoinRoom(newRoom, connection);
        }

        public void CheckRoomState()
        {
            if (this.playerCount == 0 && !isLobby)
            {
                Server.instance.RemoveRoom(this);
            }
        }

        public void JoinRoom(string roomID, Connection connection)
        {
            connection.MessageRecieved -= OnMessageRecieved;
            connections.Remove(connection);
            connection.Send(heartbeat);
            CheckRoomState();
            FindRoom(roomID).AddClient(connection);
        }

        public void JoinRoom(Room room, Connection connection)
        {
            connection.MessageRecieved -= OnMessageRecieved;
            connections.Remove(connection);
            connection.Send(heartbeat);
            CheckRoomState();
            room.AddClient(connection);
        }

        Room FindRoom(string roomID)
        {
            for (int i = 1; i < Server.instance.rooms.Count; i++)
            {
                if (Server.instance.rooms[i].roomID == roomID)
                    return Server.instance.rooms[i];
            }

            return null;
        }

        void CreateRoom(RoomCreationMessage roomCreationMessage, Connection roomCreator)
        {
            Room newRoom = new Room(roomCreationMessage.roomName, roomCreationMessage.hostName, roomCreationMessage.maxPlayerCount);
            var m = new RoomJoinMessage(roomCreator.ClientId, newRoom.roomID, 0);
            roomCreator.Send(m);
            Server.instance.rooms.Add(newRoom);
            JoinRoom(newRoom, roomCreator);
        }

        public void AddClient(Connection connection)
        {
            connection.MessageRecieved += OnMessageRecieved;
            this.connections.Add(connection);

            if (this == Server.instance.rooms[0])
            {
                SendRoomList(connection);
                return;
            }

            ClientInformationMessage m = new ClientInformationMessage(connection.ClientId, connection.Username, connections.Count - 1, 0);
            connection.Send(m);
            connection.PlayerID = connections.Count - 1;
            for (int i = 0; i < connections.Count; i++)
            {
                try
                {
                    m.informationType = 1;
                    if (connections[i] != connection)
                        connections[i].Send(m);
                    ClientInformationMessage m1 = new ClientInformationMessage(connections[i].ClientId, connections[i].Username, connections[i].PlayerID, 1);
                    connection.Send(m1);
                }
                catch (Exception e)
                {
                    ConsoleLogs.ConsoleLog(ConsoleColor.Red, e.ToString());
                    disconnectedConnections.Add(connections[i]);
                    connections[i].MessageRecieved -= OnMessageRecieved;
                }
            }
        }

        void SendRoomList(Connection connection)
        {
            RoomInformationMessage roomInformationmessage = new RoomInformationMessage();
            Room tempRoom;
            for (int i = 1; i < Server.instance.rooms.Count; i++)
            {
                tempRoom = Server.instance.rooms[i];
                roomInformationmessage.SetRoomInformations(tempRoom.roomName, tempRoom.roomID, tempRoom.roomHost, tempRoom.playerCount, tempRoom.maxPlayerNumber);
                connection.Send(roomInformationmessage);
            }
        }


        /// <summary>
        /// Terminates the Server Threads
        /// </summary>
        public void TerminateRoom()
        {
            terminateRoom = true;
        }
    }
}