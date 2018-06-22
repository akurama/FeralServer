using System.IO;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class RoomJoinMessage : MessageBase
    {
        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.RoomJoinMessage; }
        }
        
        protected override void Write(BinaryWriter w)
        {
            
        }

        protected override void Read(BinaryReader r)
        {
            
        }
    }
}