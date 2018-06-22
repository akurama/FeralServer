using System.IO;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class RoomListUpdateMessage : MessageBase
    {
        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.RoomListUpdateMessage; }
        }
        
        protected override void Write(BinaryWriter w)
        {
            
        }

        protected override void Read(BinaryReader r)
        {
            
        }
    }
}