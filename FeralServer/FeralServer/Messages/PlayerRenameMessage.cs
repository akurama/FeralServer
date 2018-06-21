using System.IO;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class PlayerRenameMessage : MessageBase
    {
        public override eMessageTypes EMessageType
        {
            get
            {
                return eMessageTypes.PlayerRenameMessage; 
            }
        }
        public string clientID;
        public string newName;
        
        protected override void Write(BinaryWriter w)
        {
            w.Write(clientID);
            w.Write(newName);
        }

        protected override void Read(BinaryReader r)
        {
            clientID = r.ReadString();
            newName = r.ReadString();
        }
    }
}