using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.InterfaceCommands.CommandModules
{
    public class ConfigCommandModule : ICommandModule
    {
        [CommandHandler(CommandType.SetConfig)]
        public void SetConfig(string parameter, string value)
        {

        }

        [CommandHandler(CommandType.SaveConfig)]
        public void SaveConfig()
        {

        }
    }
}
