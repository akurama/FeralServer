using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class ReadyMessage : MessageBase
    {
        public int readyState;
        public string clientID;
        
        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.ReadyMessage; }
        }

        protected override void Write(BinaryWriter w)
        {
            w.Write(readyState);
            w.Write(clientID);
        }

        protected override void Read(BinaryReader r)
        {
            readyState = r.ReadInt32();
            clientID = r.ReadString();
        }
    }
}
