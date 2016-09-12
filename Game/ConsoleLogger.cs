using NLua.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigeR.YuGiOh.Core
{
    public class ConsoleLogger : IMessageLogger
    {
        public bool Debugging { get; set; } = true;

        public void Assert(bool condition, string message, params object[] args)
        {
            if (!condition)
            {
                PrintError("Assertion Failed: " + message, args);
            }
        }

        public void System(string message, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message, args);
            Console.ResetColor();
        }

        public void SystemDebug(string message, params object[] args)
        {
            if (Debugging)
            {
                Console.WriteLine(message, args);
            }
        }

        public void Debug(string message, params object[] args)
        {
            if (Debugging)
            {
                Console.Write("> ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(message, args);
                Console.ResetColor();
            }
        }

        public void Print(string message, params object[] args)
        {
            Console.Write("> ");
            Console.WriteLine(message, args);
        }

        public void PrintError(string message, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message, args);
            Console.ResetColor();
        }

        public void PrintError(Exception ex)
        {
            PrintError(ex.Source + ex.Message);
        }
    }
}
