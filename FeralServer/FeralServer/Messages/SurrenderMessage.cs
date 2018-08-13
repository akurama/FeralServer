using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FeralServerProject.Collections;
using FeralServerProject.Messages;

namespace FeralServer.Messages
{
    public class SurrenderMessage : MessageBase
    {
        public string clientID;

        public SurrenderMessage()
        {

        }

        public SurrenderMessage(string clientiD)
        {
            this.clientID = clientiD;
        }

        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.SurrenderMessage; }
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
