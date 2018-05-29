using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public class StartGameMessage : MessageBase
    {
        public override MessageTypes MessageType
        {
            get { return MessageTypes.StartGameMessage; }
        }
    }
}
