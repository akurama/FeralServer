using System.IO;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class MapSendMessage : MessageBase
    {
        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.MapSendMessage; }
        }
        
        protected override void Write(BinaryWriter w)
        {
            throw new System.NotImplementedException();
        }

        protected override void Read(BinaryReader r)
        {
            throw new System.NotImplementedException();
        }
    }
}