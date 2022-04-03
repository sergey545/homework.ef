using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.Core
{
    public class AppMenuMakerWritingDevice : AppMenuMaker.IWritingDevice
    {
        
        public void WriteWithColor(string text, ConsoleColor color)
        {
            ConsoleWriter.WriteWithColor(text, color);
        }
        public void WriteLine(string text)
        {
            WriteDefault(text);
        }

        public void WriteDefault(string text) { ConsoleWriter.WriteDefault(text); }

        public void WriteRed(string text) { ConsoleWriter.WriteRed(text); }

        public void WriteGreen(string text) { ConsoleWriter.WriteGreen(text); }

        public void WriteYellow(string text) { ConsoleWriter.WriteYellow(text); }

    }
}
