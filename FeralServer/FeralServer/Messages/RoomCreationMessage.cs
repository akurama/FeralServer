using System.IO;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class RoomCreationMessage : MessageBase
    {
        public string roomName;
        public int maxPlayerCount;
        
        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.RoomCreationMessage; }
        }

        protected override void Write(BinaryWriter w)
        {
            w.Write(roomName);
            w.Write(maxPlayerCount);
        }

        protected override void Read(BinaryReader r)
        {
            roomName = r.ReadString();
            maxPlayerCount = r.ReadInt32();
        }
    }
}