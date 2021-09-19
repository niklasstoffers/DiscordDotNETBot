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
        public Task<string> ReceiveNext(CancellationToken ct)
        {
            ct.Register(() =>
            {
                Console.WriteLine();
            }, false);
            string result = Console.ReadLine();
            return Task.FromResult<string>(result);
        }
    }
}
