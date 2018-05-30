using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class HeartbeatMessage : MessageBase
    {
        public int heartbeat;

        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.HeartbeatMessage; }
        }

        protected override void Write(BinaryWriter w)
        {
            w.Write(heartbeat);
        }

        protected override void Read(BinaryReader r)
        {
            heartbeat = r.ReadInt32();
        }
    }
}
