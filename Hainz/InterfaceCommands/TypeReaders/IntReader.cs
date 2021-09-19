using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.InterfaceCommands.TypeReaders
{
    public class IntReader : TypeReader<int>
    {
        public override bool TryRead(string input, out int result) => Int32.TryParse(input, out result);
    }
}
