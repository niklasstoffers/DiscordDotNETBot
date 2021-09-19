using Autofac;
using Hainz.Framework;
using Hainz.InterfaceCommands;
using Hainz.Log;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Hainz
{
    public class Application : IDisposable
    {
        private Bot _bot;
        private Logger _logger;
        private CommandManager _commandManager;
        private ContainerManager _containerManager;
        private ILifetimeScope _container;
        private CancellationTokenSource _shutdownCTS;
        private bool _restartRequested;

        public Application()
        {
            _containerManager = new ContainerManager();   
            _container = _containerManager.Resolver;
            _shutdownCTS = new CancellationTokenSource();
        }

        public async Task Run()
        {
            try 
            {
                using (_container.BeginLifetimeScope())
                {
                    _logger = _container.Resolve<Logger>();
                    _bot = _container.Resolve<Bot>();
                    
                    if (!await TryStartBot()) return;

                    _commandManager = _container.Resolve<CommandManager>();
                    
                    _commandManager.HookHandler(CommandType.Quit, () => Quit(false));
                    _commandManager.HookHandler(CommandType.Restart, () => Quit(true));

                    await _commandManager.Listen(_shutdownCTS.Token);

                    await TryStopBot();

                    if (_restartRequested) 
                        await Restart();
                }
            }
            catch (AggregateException ex) 
            {
                await _logger?.LogExceptionAsync(ex, LogLevel.Critical);
            }
        }

        private void Quit(bool restartRequested)
        {
            _restartRequested = restartRequested;
            _shutdownCTS.Cancel();
        }

        private async Task Restart()
        {
            try 
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = Process.GetCurrentProcess().MainModule.FileName,
                    UseShellExecute = true,
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Normal
                });
            }
            catch (Exception ex) 
            {
                await _logger.LogMessageAsync("Exception while trying to restart bot.", LogLevel.Critical);
                await _logger.LogExceptionAsync(ex);
            }
        }

        private async Task<bool> TryStartBot()
        {
            try
            {
                await _bot.StartAsync();
                return true;
            }
            catch (AggregateException ex)
            {
                await _logger.LogMessageAsync("Exception while trying to start bot.", LogLevel.Critical);
                await _logger.LogExceptionAsync(ex);
                return false;
            }
        }

        private async Task<bool> TryStopBot()
        {
            try
            {
                await _bot.StopAsync();
                return true;
            }
            catch (AggregateException ex)
            {
                await _logger.LogMessageAsync("Exception while trying to shutdown bot.", LogLevel.Critical);
                await _logger.LogExceptionAsync(ex);
                return false;
            }
        }

        public void Dispose()
        {
            _container?.Dispose();
            _containerManager?.Dispose();
            _bot?.Dispose();
        }
    }
}
