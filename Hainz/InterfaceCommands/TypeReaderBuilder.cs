using Hainz.InterfaceCommands.TypeReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.InterfaceCommands
{
    public static class TypeReaderBuilder
    {
        private static TypeReader _default = new DefaultReader();
        private static Dictionary<Type, TypeReader> _readers = new Dictionary<Type, TypeReader>()
        {
            { typeof(int), new IntReader() },
            { typeof(string), new StringReader() },
            { typeof(bool), new BoolReader() }
        };

        public static TypeReader GetReader(Type type)
        {
            if (_readers.ContainsKey(type))
                return _readers[type];
            return _default;
        }
    }
}
