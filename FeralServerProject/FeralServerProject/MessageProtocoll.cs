using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralServerProject
{
    public class MessageProtocoll
    {
        public event Action<byte[]> MessageComplete;

        private byte[] collecionBuffer = new byte[1024*64];
    }
}
