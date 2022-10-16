
using System;

namespace Otus.Teaching.PromoCodeFactory.Core
{
    public class ConsoleToApiMessage : BaseEntity
    {
        public ConsoleToApiMessage (string text="")
        {
            Text = text;
        }
        public ConsoleToApiMessage()
        {

        }
        public string Text { get; set; }

        public override string ToString()
        {
            return $"{Text}      ---- {ShortTimeStamp} {SysMessage}";
        }
    }
}