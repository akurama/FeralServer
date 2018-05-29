using System;
using System.Collections.Generic;
using System.Globalization;
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

            //ToDo: Format String
            string output = "[" + dateTime.Day + "." + dateTime.Month + "." + dateTime.Year + " " + dateTime.Hour + ":" + dateTime.Minute + ":" + dateTime.Second + "] " + message;

            Console.ForegroundColor = color;
            Console.WriteLine(output);
            Console.ResetColor();
            
        }

        public static void ConsoleLog(string message)
        {
            //String: [Day.Month.Year HH:MM:SS] + Message
            DateTime dateTime = DateTime.Now;

            //ToDo: Format String
            string output = "[" + dateTime.Day + "." + dateTime.Month + "." + dateTime.Year + " " + dateTime.Hour + ":" + dateTime.Minute + ":" + dateTime.Second + "] " + message;
            Console.WriteLine(output);
        }

        void WriteToFile(string message)
        {
            // TODO: FileOutPut
        }
    }
}
