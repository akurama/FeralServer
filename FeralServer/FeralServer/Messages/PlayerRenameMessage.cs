using System.IO;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class PlayerRenameMessage : MessageBase
    {
        public override eMessageTypes EMessageType { get; }
        public string clientID;
        public string newName;
        
        protected override void Write(BinaryWriter w)
        {
            
        }

        protected override void Read(BinaryReader r)
        {
            
        }
    }
}