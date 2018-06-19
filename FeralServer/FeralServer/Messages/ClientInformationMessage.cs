using System.IO;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class ClientInformationMessage : MessageBase
    {
        public string clientID;
        public int playerID;

        public ClientInformationMessage()
        {
            
        }
        
        public ClientInformationMessage(string clientID, int playerID)
        {
            this.clientID = clientID;
            this.playerID = playerID;
        }
        
        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.ClientInformationMessage; }
        }
        
        protected override void Write(BinaryWriter w)
        {
            w.Write(clientID);
            w.Write(playerID);
        }

        protected override void Read(BinaryReader r)
        {
            clientID = r.ReadString();
            playerID = r.ReadInt16();
        }
    }
}