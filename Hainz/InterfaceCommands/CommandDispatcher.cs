using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.InterfaceCommands
{
    public class CommandDispatcher
    {
        private ILifetimeScope _container;

        public CommandDispatcher(ILifetimeScope container)
        {
            _container = container;
        }

        public async Task Dispatch(Type moduleType, InvokeHandler handler, Command command)
        {
            ICommandModule module = null;
            if (moduleType != null)
                module = _container.Resolve(moduleType) as ICommandModule;

            var dispatcherTask = Task.Run(() =>
            {
                try
                {
                    handler.Invoke(module, command.Parameters);
                }
                catch { }
            });

            await dispatcherTask;
        }
    }
}
