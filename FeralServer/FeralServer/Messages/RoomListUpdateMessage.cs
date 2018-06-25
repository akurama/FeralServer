using System.IO;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class RoomListUpdateMessage : MessageBase
    {
        public string clientID;

        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.RoomListUpdateMessage; }
        }
        
        protected override void Write(BinaryWriter w)
        {
            w.Write(clientID);
        }

        protected override void Read(BinaryReader r)
        {
            clientID = r.ReadString();
        }
    }
}