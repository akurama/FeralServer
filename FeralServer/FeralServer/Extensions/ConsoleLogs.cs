using System;
using System.Collections.Generic;
using System.Data;
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

            GetDateString();
            
            string output = "[" + GetDateString() + "] " + message;
            
            Console.ForegroundColor = color;
            Console.WriteLine(output);
            Console.ResetColor();
        }

        public static void ConsoleLog(string message)
        {
            //String: [Day.Month.Year HH:MM:SS] + Message
            DateTime dateTime = DateTime.Now;

            string output = "[" + GetDateString() + "] " + message;
            Console.WriteLine(output);
        }

        static string GetDateString()
        {
            DateTime dateTime = DateTime.Now;

            return dateTime.ToString().PadLeft(1);
        }

        void WriteToFile(string message)
        {
            // TODO: FileOutPut
        }
    }
}
