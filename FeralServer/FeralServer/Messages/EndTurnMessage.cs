using System.IO;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class EndTurnMessage : MessageBase
    {
        public int playerIDold;
        public int playerIDnew;
        public int turnIDold;
        
        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.EndTurnMessage; }
        }
        protected override void Write(BinaryWriter w)
        {
            w.Write(playerIDold);
            w.Write(playerIDnew);
            w.Write(turnIDold);
        }

        protected override void Read(BinaryReader r)
        {
            playerIDold = r.ReadInt32();
            playerIDnew = r.ReadInt32();
            turnIDold = r.ReadInt32();
        }
    }
}