using System.IO;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class RoomJoinMessage : MessageBase
    {
        public string clientID;
        public string roomID;
        public int result;

        public RoomJoinMessage()
        {

        }

        public RoomJoinMessage(string clientID, string roomID, int result)
        {
            this.clientID = clientID;
            this.roomID = roomID;
            this.result = result;
        }

        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.RoomJoinMessage; }
        }

        protected override void Write(BinaryWriter w)
        {
            w.Write(clientID);
            w.Write(roomID);
            w.Write(result);
        }

        protected override void Read(BinaryReader r)
        {
            clientID = r.ReadString();
            roomID = r.ReadString();
            result = r.ReadInt32();
        }
    }
}