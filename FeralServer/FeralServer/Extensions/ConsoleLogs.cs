using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using FeralServerProject.Messages;
using FeralServerProject.Collections;

namespace FeralServerProject.Extensions
{
    public class ConsoleLogs
    {
        public static void ConsoleLog(ConsoleColor color = ConsoleColor.White, string message = "", bool FileOutput = true)
        {
            //String: [Day.Month.Year HH:MM:SS] + Message
            DateTime dateTime = DateTime.Now;

            GetDateString();
            
            string output = "[" + GetDateString() + "] " + message;
            
            Console.ForegroundColor = color;
            Console.WriteLine(output);
            if(FileOutput)
                WriteToFile(output);
            Console.ResetColor();
        }

        public static void ConsoleLog(string message, bool FileOutput)
        {
            //String: [Day.Month.Year HH:MM:SS] + Message
            DateTime dateTime = DateTime.Now;

            string output = "[" + GetDateString() + "] " + message;
            if(FileOutput)
                WriteToFile(output);
            Console.WriteLine(output);
        }

        static string GetDateString()
        {
            DateTime dateTime = DateTime.Now;

            return dateTime.ToString().PadLeft(1);
        }

        static void WriteToFile(string message)
        {
            string path = @"./serveroutput.txt";

            try
            {
                FileStream fs = null;
                message += "\n";

                if (File.Exists(path))
                {
                    using (fs = File.Open(path, FileMode.Append, FileAccess.Write))
                    {
                        Write(fs, message);
                        return;
                    }
                }
                else
                {
                    fs = File.Create(path);
                    Write(fs, message);
                }
            }
            catch (Exception e)
            {
                
            }
        }

        static void Write(FileStream fs, string message)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(message);
            fs.Write(info, 0, info.Length);
            fs.Close();
        }
        
        public static void LogMessage(MessageBase message)
        {
            if (message is ConnectMessage)
            {
                var m = (ConnectMessage) message;
                ConsoleLogs.ConsoleLog(ConsoleColor.Green, m.userName + " has connected");
            }
            else if (message is DisconnectMessage)
            {
                var m = (DisconnectMessage) message;
                ConsoleLogs.ConsoleLog(ConsoleColor.DarkGreen, "Client with the ID" + m.clientID + " has disconnected.");
            }
            else if (message is ReadyMessage)
            {
                var m = (ReadyMessage) message;
                eReadyState readyState = (eReadyState) m.readyState;
                ConsoleLogs.ConsoleLog(ConsoleColor.Magenta, "Player with the client id " + m.clientID + 
                                                             (readyState == eReadyState.Ready ? " is Ready" : " is not Ready"));
            }
            else if (message is StartGameMessage)
            {
                var m = (StartGameMessage) message;
                ConsoleLogs.ConsoleLog(ConsoleColor.DarkCyan, "Game starts now!");
            }
            else if (message is StopGameMessage)
            {
                var m = (StopGameMessage) message;
            }
            else if (message is ChatMessage)
            {
                var m = (ChatMessage) message;
                ConsoleLogs.ConsoleLog(ConsoleColor.Blue, m.clientID + ": " + m.messageText);
            }
            else if (message is HeartbeatMessage)
            {
                var m = (HeartbeatMessage) message;
            }
            else if (message is GameStateMessage)
            {
                var m = (GameStateMessage) message;
            }
            else if (message is GameInputMessage)
            {
                var m = (GameInputMessage) message;
                ConsoleLogs.ConsoleLog(ConsoleColor.DarkCyan, "From Cell: " + m.fromCellIndex + " To Cell: " + m.toCellIndex + " Interaction Type: " + m.interactionType);
            }
            else if (message is EndTurnMessage)
            {
                var m = (EndTurnMessage) message;
                ConsoleLogs.ConsoleLog(ConsoleColor.DarkYellow, "Player " + m.playerIDold + " finished Turn " + m.turnIDold + " now its Player " + m.playerIDnew + " turn");
            }   
            else if (message is GameSettingsMessage)
            {
                var m = (GameInputMessage) message;
            }
            else if (message is PlayerRenameMessage)
            {
                var m = (PlayerRenameMessage) message;
                ConsoleLog(ConsoleColor.Blue, "Client " + m.clientID + " renamed to " + m.newName);
            }
            else if(message is RoomCreationMessage)
            {
                var m = (RoomCreationMessage) message;
            }
            else if (message is RoomInformationMessage)
            {
                var m = (RoomInformationMessage) message;
            }
            else if (message is RoomJoinMessage)
            {
                var m = (RoomJoinMessage) message;
            }
            else if (message is RoomListUpdateMessage)
            {
                var m = (RoomListUpdateMessage) message;
            }
            else if (message is MapSendMessage)
            {
                var m = (MapSendMessage) message;
            }
        }
    }
}
