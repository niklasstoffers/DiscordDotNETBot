using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.InterfaceCommands
{
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    sealed class CommandHandlerAttribute : Attribute
    { 
        public CommandType ForType { get; init; }
        public CommandHandlerAttribute(CommandType forType)
        {
            ForType = forType;
        }
    }
}
