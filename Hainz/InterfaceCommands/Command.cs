using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.InterfaceCommands
{
    public class Command
    {
        public CommandType Type { get; init; }
        public string[] Parameters { get; init; }

        public Command(CommandType type, string[] parameters)
        {
            Type = type;
            Parameters = parameters;
        }
    }
}
