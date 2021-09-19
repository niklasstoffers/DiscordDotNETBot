using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.InterfaceCommands
{
    public class CommandHandler
    {
        public Type ModuleType { get; init; }
        public InvokeHandler Handler { get; init; }

        public CommandHandler(Type moduleType, InvokeHandler handler)
        {
            ModuleType = moduleType;
            Handler = handler;
        }
    }
}
