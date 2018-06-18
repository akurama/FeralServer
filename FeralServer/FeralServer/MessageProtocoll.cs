using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralServerProject
{
    public class MessageProtocoll
    {
        public event Action<byte[]> MessageComplete;

        private byte[] collecionBuffer = new byte[1024*64];
        private int bytesCollected = 0;
        private int messageSize = 0;

        public void ReceiveData(byte[] newData, int start, int count)
        {
            for (int i = start; i < start + count; ++i)
            {
                this.collecionBuffer[this.bytesCollected] = newData[i];
                this.bytesCollected++;

                if (this.bytesCollected == 4)
                {
                    BinaryReader binaryReader = new BinaryReader(new MemoryStream(this.collecionBuffer));
                    this.messageSize = binaryReader.ReadInt32();

                    if (this.messageSize > this.collecionBuffer.Length)
                    {
                        //UPS I DID IT AGAIN
                        //Message to big for the buffer this is a serious problem
                    }
                }

                if (this.messageSize > 0 && this.bytesCollected == this.messageSize)
                {
                    byte[] b = new byte[this.messageSize];
                    Array.Copy(this.collecionBuffer, 0, b, 0, this.messageSize);
                    this.MessageComplete(b);

                    this.bytesCollected = 0;
                    this.messageSize = 0;
                }
            }
        }
    }
}
