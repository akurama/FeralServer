using System.IO;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class RoomInformationMessage : MessageBase
    {
        public string roomName;
        public string roomID;
        public string hostUserName;
        public int currentPlayerCount;
        public int maxPlayerCount;
        
        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.RoomInformationMessage; }
        }
        
        public RoomInformationMessage()
        {
            
        }

        public RoomInformationMessage(string roomName, string roomID, string hostUserName, int currentPlayerCount,
            int maxPlayerCount)
        {
            this.roomName = roomName;
            this.roomID = roomID;
            this.hostUserName = hostUserName;
            this.currentPlayerCount = currentPlayerCount;
            this.maxPlayerCount = maxPlayerCount;
        }
        
        protected override void Write(BinaryWriter w)
        {
            w.Write(roomName);
            w.Write(roomID);
            w.Write(hostUserName);
            w.Write(currentPlayerCount);
            w.Write(maxPlayerCount);
        }

        protected override void Read(BinaryReader r)
        {
            roomName = r.ReadString();
            roomID = r.ReadString();
            hostUserName = r.ReadString();
            currentPlayerCount = r.ReadInt32();
            maxPlayerCount = r.ReadInt32();
        }

        public void SetRoomInformations(string roomName, string roomID, string hostUserName, int currentPlayerCount,
            int maxPlayerCount)
        {
            this.roomName = roomName;
            this.roomID = roomID;
            this.hostUserName = hostUserName;
            this.currentPlayerCount = currentPlayerCount;
            this.maxPlayerCount = maxPlayerCount;
        }
    }
}