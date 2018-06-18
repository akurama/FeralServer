using System.IO;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class GameSettingsMessage : MessageBase
    {
        public int playerID;
        public int settingsID;
        public int newValue;
        
        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.GameSettingsMessage; }
        }
        
        protected override void Write(BinaryWriter w)
        {
            w.Write(playerID);
            w.Write(settingsID);
            w.Write(newValue);
        }

        protected override void Read(BinaryReader r)
        {
            playerID = r.ReadInt32();
            settingsID = r.ReadInt32();
            newValue = r.ReadInt32();
        }
    }
}