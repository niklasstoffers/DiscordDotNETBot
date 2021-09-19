using Autofac;
using Hainz.Log;
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
        private Logger _logger;

        public CommandDispatcher(ILifetimeScope container, Logger logger)
        {
            _container = container;
            _logger = logger;
        }

        public async Task Dispatch(Type moduleType, InvokeHandler handler, Command command)
        {
            ICommandModule module = null;
            try
            {
                if (moduleType != null)
                    module = _container.Resolve(moduleType) as ICommandModule;
            }
            catch (Exception ex)
            {
                await _logger.LogMessageAsync("Exception while creating interface command module.", LogLevel.Error);
                await _logger.LogExceptionAsync(ex);
                return;
            }

            var dispatcherTask = Task.Run(async () =>
            {
                try
                {
                    handler.Invoke(module, command.Parameters);
                }
                catch(Exception ex) 
                {
                    await _logger.LogMessageAsync("Exception while trying to invoke interface command handler.", LogLevel.Error);
                    await _logger.LogExceptionAsync(ex);
                }
            });

            await dispatcherTask;
        }
    }
}
