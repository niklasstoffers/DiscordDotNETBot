using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.InterfaceCommands
{
    public class CommandHandlerLocator
    {
        private Lazy<IEnumerable<Type>> _commandModules;
        private Dictionary<CommandType, List<CommandHandler>> _handlers;

        public CommandHandlerLocator()
        {
            _commandModules = new Lazy<IEnumerable<Type>>(() => LocateModules());
            _handlers = new Dictionary<CommandType, List<CommandHandler>>();
        }

        private IEnumerable<Type> LocateModules()
        {
            var assemblyTypes = Assembly.GetExecutingAssembly().GetTypes();
            return assemblyTypes.Where(x => x.IsAssignableTo(typeof(ICommandModule)));
        }

        public List<CommandHandler> GetCommandHandlers(CommandType commandType)
        {
            if (_handlers.ContainsKey(commandType))
                return _handlers[commandType];
            _handlers.Add(commandType, new List<CommandHandler>());

            foreach (var module in _commandModules.Value)
            {
                var methodMembers = module.GetMethods(BindingFlags.Instance | BindingFlags.Public);
                var commandHandlers = methodMembers.Where(m => m.GetCustomAttribute<CommandHandlerAttribute>()?.ForType == commandType);
                foreach (var handler in commandHandlers)
                    _handlers[commandType].Add(new CommandHandler(module, new HandlerInvoker(handler).Invoker));
            }

            return _handlers[commandType];
        }
    }
}
