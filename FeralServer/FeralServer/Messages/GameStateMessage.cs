﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class GameStateMessage : MessageBase
    {
        public override eMessageTypes EMessageType
        {
            get { return eMessageTypes.GameStateMessage; }
        }

        protected override void Write(BinaryWriter w)
        {
            
        }

        protected override void Read(BinaryReader r)
        {
            
        }
    } 
}
