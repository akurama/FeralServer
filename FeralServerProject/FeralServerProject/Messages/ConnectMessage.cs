using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class ConnectMessage : MessageBase
    {
        public string senderName;

        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.ConnnectMessage; }
        }

        protected override void Write(BinaryWriter w)
        {
            w.Write(senderName);
        }

        protected override void Read(BinaryReader r)
        {
            senderName = r.ReadString();
        }
    }
}
