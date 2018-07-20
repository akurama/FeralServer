using System.IO;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class GameSettingsMessage : MessageBase
    {
        public int playerID;
        public string clientID;
        public int settingsID;
        public int newValue;

        public GameSettingsMessage()
        {

        }

        public GameSettingsMessage(int playerID, string clientID, int settingsID, int newValue)
        {
            this.playerID = playerID;
            this.clientID = clientID;
            this.settingsID = settingsID;
            this.newValue = newValue;
        }
        
        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.GameSettingsMessage; }
        }

        protected override void Write(BinaryWriter w)
        {
            w.Write(playerID);
            w.Write(clientID);
            w.Write(settingsID);
            w.Write(newValue);
        }

        protected override void Read(BinaryReader r)
        {
            playerID = r.ReadInt32();
            clientID = r.ReadString();
            settingsID = r.ReadInt32();
            newValue = r.ReadInt32();
        }
    }
}