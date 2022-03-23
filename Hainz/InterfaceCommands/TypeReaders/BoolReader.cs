using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.InterfaceCommands.TypeReaders
{
    public class BoolReader : TypeReader<bool>
    {
        public override bool TryRead(string input, out bool result) => Boolean.TryParse(input, out result);
    }
}
