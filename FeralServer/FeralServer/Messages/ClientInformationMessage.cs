using System.IO;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class ClientInformationMessage : MessageBase
    {
        public string clientID;
        public string userName;
        public int playerID;
        public int informationType;
        

        public ClientInformationMessage()
        {
            
        }
        
        public ClientInformationMessage(string clientID, string userName, int playerID, int informationType)
        {
            this.clientID = clientID;
            this.userName = userName;
            this.playerID = playerID;
            this.informationType = informationType;
        }
        
        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.ClientInformationMessage; }
        }
        
        protected override void Write(BinaryWriter w)
        {
            w.Write(clientID);
            w.Write(userName);
            w.Write(playerID);
            w.Write(informationType);
        }

        protected override void Read(BinaryReader r)
        {
            clientID = r.ReadString();
            userName = r.ReadString();
            playerID = r.ReadInt32();
            informationType = r.ReadInt32();
        }
    }
}