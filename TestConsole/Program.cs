using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TigeR.YuGiOh.Core;
using TigeR.YuGiOh.Core.Players;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(new DefaultPlayer(), new DefaultPlayer(), new ConsoleLogger());
            game.RunAsync().Wait();

            Console.WriteLine("TERMINATED");
            Console.ReadKey();
        }
    }
}
