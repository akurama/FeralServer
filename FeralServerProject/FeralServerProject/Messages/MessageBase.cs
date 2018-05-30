using System;
using System.IO;
using FeralServerProject.Collections;
using FeralServerProject.Extensions;

namespace FeralServerProject.Messages
{
    public abstract class MessageBase
    {
        public abstract MessageTypes MessageType { get; }

        protected abstract void Write(BinaryWriter w);
        protected abstract void Read(BinaryReader r);

        public byte[] ToByteArray()
        {
            MemoryStream memorySteam = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memorySteam);

            binaryWriter.Write((int) 0);
            binaryWriter.Write((int)this.MessageType);

            Write(binaryWriter);

            int size = (int)memorySteam.Length;
            memorySteam.Seek(0, SeekOrigin.Begin);
            binaryWriter.Write(size);

            return memorySteam.ToArray();
        }

        public static MessageBase FromByteArray(byte[] b)
        {
            MemoryStream memoryStream = new MemoryStream(b);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            int size = binaryReader.ReadInt32();
            MessageTypes messageType = (MessageTypes) binaryReader.ReadInt32();

            MessageBase m;
            switch (messageType)
            {
                case MessageTypes.ConnnectMessage:
                    m = new ChatMessage();
                    break;
                case MessageTypes.DisconnectMessage:
                    m = new DisconnectMessage();
                    break;
                case MessageTypes.ReadyMessage:
                    m = new ReadyMessage();
                    break;
                case MessageTypes.StartGameMessage:
                    m = new StartGameMessage();
                    break;
                case MessageTypes.StopGameMessage:
                    m = new StopGameMessage();
                    break;
                case MessageTypes.ChatMessage:
                    m = new ChatMessage();
                    break;
                case MessageTypes.HeartbeatMessage:
                    m = new HeartbeatMessage();
                    break;
                case MessageTypes.EmptyMessage2:
                    ConsoleLogs.ConsoleLog(ConsoleColor.Red, "Message Type not Implemented");
                    return null;
                case MessageTypes.GameStateMessage:
                    m = new GameStateMessage();
                    break;
                case MessageTypes.GameInputMessage:
                    m = new GameInputMessage();
                    break;
                default:
                    //TODO: DONT LET THE SERVER CRASH
                    throw new ArgumentOutOfRangeException();
            }

            m.Read(binaryReader);

            return m;
        }
    }
}
