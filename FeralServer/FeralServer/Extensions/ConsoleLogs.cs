using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
    }
}
