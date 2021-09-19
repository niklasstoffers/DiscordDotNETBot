using Hainz.IO;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hainz.InterfaceCommands
{
    public class CommandManager
    {
        private CommandDispatcher _dispatcher;
        private CommandHandlerLocator _locator;
        private CommandParser _parser;
        private IInput _input;
        private Dictionary<CommandType, List<CommandHandler>> _handlers;
        private SemaphoreSlim _handlerLock = new SemaphoreSlim(1, 1);

        public CommandManager(CommandDispatcher dispatcher,
                                     CommandHandlerLocator locator,
                                     CommandParser parser,
                                     IInput input)
        {
            _dispatcher = dispatcher;
            _locator = locator;
            _parser = parser;
            _input = input;
            _handlers = new Dictionary<CommandType, List<CommandHandler>>();
        }

        public void HookHandler(CommandType type, Action handler)
        {
            EnsureInit(type);
            _handlerLock.Wait();
            try {
                _handlers[type].Add(new CommandHandler(null, (m, p) => handler()));
            }
            finally {
                _handlerLock.Release();
            }
        }

        private void EnsureInit(CommandType type)
        {
            _handlerLock.Wait();
            try {
                if (!_handlers.ContainsKey(type))
                    _handlers.Add(type, _locator.GetCommandHandlers(type));
            }
            finally {
                _handlerLock.Release();
            }
        }

        public async Task Listen(CancellationToken cancelToken)
        {
            while(!cancelToken.IsCancellationRequested)
            {
                string input = await _input.ReceiveNext(cancelToken);
                if (cancelToken.IsCancellationRequested)
                    break;

                Command command = _parser.ParseCommand(input);
                if (command == null) continue;
                EnsureInit(command.Type);

                foreach (var handler in _handlers[command.Type])
                    await _dispatcher.Dispatch(handler.ModuleType, handler.Handler, command);
            }
        }

        public async Task DispatchCommand(Command command) 
        {
            EnsureInit(command.Type);
            foreach (var handler in _handlers[command.Type])
                await _dispatcher.Dispatch(handler.ModuleType, handler.Handler, command);
        }
    }
}
