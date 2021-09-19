using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.InterfaceCommands.TypeReaders
{
    public class DefaultReader : TypeReader
    {
        public override bool TryRead(string input, out object result)
        {
            result = null;
            return false;
        }
    }
}
