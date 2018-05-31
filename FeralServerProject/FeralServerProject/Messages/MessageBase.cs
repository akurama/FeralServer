using System;
using System.IO;
using FeralServerProject.Collections;
using FeralServerProject.Extensions;

namespace FeralServerProject.Messages
{
    public abstract class MessageBase
    {
        public abstract eMessageTypes EMessageType { get; }

        protected abstract void Write(BinaryWriter w);
        protected abstract void Read(BinaryReader r);

        public byte[] ToByteArray()
        {
            MemoryStream memorySteam = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(memorySteam);

            binaryWriter.Write((int) 0);
            binaryWriter.Write((int)this.EMessageType);

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
            eMessageTypes eMessageType = (eMessageTypes) binaryReader.ReadInt32();

            MessageBase m;
            switch (eMessageType)
            {
                case eMessageTypes.ConnnectMessage:
                    m = new ConnectMessage();
                    break;
                case eMessageTypes.DisconnectMessage:
                    m = new DisconnectMessage();
                    break;
                case eMessageTypes.ReadyMessage:
                    m = new ReadyMessage();
                    break;
                case eMessageTypes.StartGameMessage:
                    m = new StartGameMessage();
                    break;
                case eMessageTypes.StopGameMessage:
                    m = new StopGameMessage();
                    break;
                case eMessageTypes.ChatMessage:
                    m = new ChatMessage();
                    break;
                case eMessageTypes.HeartbeatMessage:
                    m = new HeartbeatMessage();
                    break;
                case eMessageTypes.EmptyMessage2:
                    ConsoleLogs.ConsoleLog(ConsoleColor.Red, "Message Type not Implemented");
                    return null;
                case eMessageTypes.GameStateMessage:
                    m = new GameStateMessage();
                    break;
                case eMessageTypes.GameInputMessage:
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
