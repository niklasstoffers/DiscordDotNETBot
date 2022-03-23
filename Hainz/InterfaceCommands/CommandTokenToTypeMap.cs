using Hainz.InterfaceCommands.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.InterfaceCommands
{
    public static class CommandTokenToTypeMap
    {
        private static readonly Dictionary<CommandCategory, Dictionary<CommandName, CommandType>> _map = new Dictionary<CommandCategory, Dictionary<CommandName, CommandType>>()
        {
            {
                CommandCategory.None,
                new Dictionary<CommandName, CommandType>()
                {
                    { CommandName.Quit, CommandType.Quit },
                    { CommandName.Restart, CommandType.Restart }
                }
            },
            {
                CommandCategory.Config,
                new Dictionary<CommandName, CommandType>()
                {
                    { CommandName.Save, CommandType.SaveConfig },
                    { CommandName.Set, CommandType.SetConfig }
                }
            }
        };

        public static CommandType? GetCommandType(CommandCategory category, CommandName commandName)
        {
            if (_map.ContainsKey(category) && _map[category].ContainsKey(commandName))
                return _map[category][commandName];
            return null;
        }
    }
}
