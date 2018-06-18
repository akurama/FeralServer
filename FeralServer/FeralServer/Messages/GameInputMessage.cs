using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class GameInputMessage : MessageBase
    {
        public int fromCellIndex;
        public int toCellIndex;
        public int interactionType;
        
        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.GameInputMessage; }
        }

        protected override void Write(BinaryWriter w)
        {
            w.Write(this.fromCellIndex);
            w.Write(this.toCellIndex);
            w.Write(this.interactionType);
        }

        protected override void Read(BinaryReader r)
        {
            this.fromCellIndex = r.ReadInt32();
            this.toCellIndex = r.ReadInt32();
            this.interactionType = r.ReadInt32();
        }
    }
}
