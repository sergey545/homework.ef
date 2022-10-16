using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.Core
{
    public static class ConsoleWriter
    {
        //класс, который пишет в консоль разными цветами и пр.
        private static readonly object _locker = new object();
        public static void WriteWithColor(string text, ConsoleColor color)
        {
            lock (_locker) //если не использовать локер, цвета смешиваются
            {
                ConsoleColor _color = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(text);
                Console.WriteLine("");
                Console.ForegroundColor = _color;
            }
        }

        public static void WriteDefault(string text) { WriteWithColor(text, Console.ForegroundColor); }

        public static void WriteRed(string text) { WriteWithColor(text, ConsoleColor.Red); }

        public static void WriteGreen(string text) { WriteWithColor(text, ConsoleColor.Green); }

        public static void WriteYellow(string text) { WriteWithColor(text, ConsoleColor.Yellow); }

    }
}
