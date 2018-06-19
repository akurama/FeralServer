using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralServerProject.Extensions
{
    public class ConsoleLogs
    {
        public static void ConsoleLog(ConsoleColor color = ConsoleColor.White, string message = "")
        {
            //String: [Day.Month.Year HH:MM:SS] + Message
            DateTime dateTime = DateTime.Now;

            GetDateString();
            
            string output = "[" + GetDateString() + "] " + message;
            
            Console.ForegroundColor = color;
            Console.WriteLine(output);
            WriteToFile(output);
            Console.ResetColor();
        }

        public static void ConsoleLog(string message)
        {
            //String: [Day.Month.Year HH:MM:SS] + Message
            DateTime dateTime = DateTime.Now;

            string output = "[" + GetDateString() + "] " + message;
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

            if (File.Exists(path))
            {
                
            }
            else
            {
                File.Create(path);
            }

            message += "\n";

            using (FileStream fs = File.Open(path, FileMode.Append, FileAccess.Write))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(message);
                fs.Write(info, 0, info.Length);
                fs.Close();
            }
        }
    }
}
