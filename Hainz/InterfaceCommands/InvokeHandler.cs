using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.InterfaceCommands
{
    public delegate void InvokeHandler(ICommandModule module, string[] parameters);
}
