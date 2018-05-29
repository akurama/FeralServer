using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeralServerProject.Collections;

namespace FeralServerProject.Messages
{
    public abstract class MessageBase
    {
        public abstract MessageTypes MessageType { get; }
    }
}
