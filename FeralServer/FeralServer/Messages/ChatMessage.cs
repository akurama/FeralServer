using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class ChatMessage : MessageBase
    {
        public string SenderName;
        public string messageText;

        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.ChatMessage; }
        }

        protected override void Write(BinaryWriter w)
        {
            w.Write(this.SenderName);
            w.Write(this.messageText);
        }

        protected override void Read(BinaryReader r)
        {
            this.SenderName = r.ReadString();
            this.messageText = r.ReadString();
        }
    }
}
