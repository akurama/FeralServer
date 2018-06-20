using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class StartGameMessage : MessageBase
    {
        public string clientID;
        
        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.StartGameMessage; }
        }

        protected override void Write(BinaryWriter w)
        {
            w.Write(clientID);
        }

        protected override void Read(BinaryReader r)
        {
            clientID = r.ReadString();
        }
    }
}
