using System;
using System.Threading.Tasks;
using Discord.Commands;
using Hainz.IO;

namespace Hainz.Commands.Modules 
{
    [Group("bot")]
    public class BotModule : ModuleBase<SocketCommandContext>
    {
        private Hainz.InterfaceCommands.CommandManager _commandManager;
        private Hainz.InterfaceCommands.CommandBuilder _commandBuilder;

        public BotModule(Hainz.InterfaceCommands.CommandManager commandManager,
                            Hainz.InterfaceCommands.CommandBuilder commandBuilder) 
        {
            _commandManager = commandManager;
            _commandBuilder = commandBuilder;
        }

        [Command("quit", RunMode = RunMode.Async)]
        public async Task QuitAsync() 
        {
            var command = _commandBuilder.WithType(InterfaceCommands.CommandType.Quit).Build();
            await _commandManager.DispatchCommand(command);
        }

        [Command("restart", RunMode = RunMode.Async)]
        public async Task RestartAsync() 
        {
            var command = _commandBuilder.WithType(InterfaceCommands.CommandType.Restart).Build();
            await _commandManager.DispatchCommand(command);
        }

        [Group("config")]
        public class ConfigModule : ModuleBase<SocketCommandContext>
        {
            private Hainz.InterfaceCommands.CommandManager _commandManager;
            private Hainz.InterfaceCommands.CommandBuilder _commandBuilder;

            public ConfigModule(Hainz.InterfaceCommands.CommandManager commandManager,
                                Hainz.InterfaceCommands.CommandBuilder commandBuilder) 
            {
                _commandManager = commandManager;
                _commandBuilder = commandBuilder;
            }

            [Command("set", RunMode = RunMode.Async)]
            public async Task SetAsync(params string[] parameters) 
            {
                var command = _commandBuilder.WithType(InterfaceCommands.CommandType.SetConfig)
                                             .WithParameters(parameters)
                                             .Build();
                await _commandManager.DispatchCommand(command);
            }

            [Command("save", RunMode = RunMode.Async)]
            public async Task SaveAsync() 
            {
                var command = _commandBuilder.WithType(InterfaceCommands.CommandType.SaveConfig).Build();
                await _commandManager.DispatchCommand(command);
            }
        }
    }
}