using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TigeR.YuGiOh.Core;
using TigeR.YuGiOh.Core.Data;
using TigeR.YuGiOh.Core.Players;
using TigeR.YuGiOh.UI.Rendering;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Game game = new Game(new DefaultPlayer(), new DefaultPlayer(), new ConsoleLogger());
            //game.RunAsync().Wait();

            var cl = new CardLoader();
            var a = new CardGenerator();

            foreach (var file in Directory.GetFiles("../../../Cards/", "*.card"))
            {
                var c = cl.LoadFromFile(file);
                var b = a.Render(c);

                b.Save("../../../Cards/" + Path.GetFileName(file) + ".bmp");
            }

            Console.WriteLine("TERMINATED");
            //Console.ReadKey();
        }
    }
}
