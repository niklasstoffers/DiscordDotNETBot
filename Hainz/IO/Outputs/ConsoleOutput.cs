using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.IO.Outputs
{
    public class ConsoleOutput : OutputBase
    {
        public override Task AppendAsync(string message)
        {
            ConsoleColor color = CurrentContextType switch
            {
                OutputContextType.Critical => ConsoleColor.DarkRed,
                OutputContextType.Error => ConsoleColor.Red,
                OutputContextType.Warning => ConsoleColor.DarkYellow,
                OutputContextType.Debug => ConsoleColor.Blue,
                _ => ConsoleColor.White
            };

            Console.ForegroundColor = color;
            Console.WriteLine(message);
            return Task.CompletedTask;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
