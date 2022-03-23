using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.InterfaceCommands.TypeReaders
{
    public class StringReader : TypeReader<string>
    {
        public override bool TryRead(string input, out string result)
        {
            result = input;
            return true;
        }
    }
}
