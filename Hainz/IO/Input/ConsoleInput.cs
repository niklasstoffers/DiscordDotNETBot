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
        public string ReceiveNext(CancellationToken ct)
        {
            ct.Register(() =>
            {
                Console.WriteLine();
            }, false);
            return Console.ReadLine();
        }
    }
}
