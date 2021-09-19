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
                if (key.Key == ConsoleKey.Enter) break;
                input.Add(key.KeyChar);
            }
            return new string(input.ToArray());
        }
    }
}
