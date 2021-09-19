using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.InterfaceCommands.TypeReaders
{
    public abstract class TypeReader
    {
        public abstract bool TryRead(string input, out object result);
    }

    public abstract class TypeReader<T> : TypeReader
    {
        public abstract bool TryRead(string input, out T result);

        public sealed override bool TryRead(string input, out object result)
        {
            bool successfull = TryRead(input, out T tResult);
            result = tResult;
            return successfull;
        }
    }
}
