using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hainz.IO.Input
{
    public class ConsoleInput : IInput
    {
        public async Task<string> ReceiveNext(CancellationToken ct)
        {
            List<char> input = new List<char>();
            while(!ct.IsCancellationRequested)
            {
                while (!(Console.KeyAvailable || ct.IsCancellationRequested))
                    await Task.Delay(10);
                if (ct.IsCancellationRequested) return null;
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.Write(Environment.NewLine);
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (input.Count > 0)
                    {
                        input.RemoveAt(input.Count - 1);
                        Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                        Console.Write("\b \b");
                    }
                }
                else
                    input.Add(key.KeyChar);
            }
            return new string(input.ToArray());
        }
    }
}
