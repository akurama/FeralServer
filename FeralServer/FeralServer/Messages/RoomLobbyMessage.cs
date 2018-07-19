using System.IO;
using FeralServerProject.Collections;
using FeralServerProject.Messages;

namespace FeralServer.Messages
{
    public class RoomLobbyMessage : MessageBase
    {
        public string clientID;
        public int result;

        public RoomLobbyMessage()
        {

        }

        public RoomLobbyMessage(string clientID, int result)
        {
            this.clientID = clientID;
            this.result = result;
        }

        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.RoomLobbyMessage; }
        }

        protected override void Read(BinaryReader r)
        {
            clientID = r.ReadString();
            result = r.ReadInt32();
        }

        protected override void Write(BinaryWriter w)
        {
            w.Write(clientID);
            w.Write(result);
        }
    }
}