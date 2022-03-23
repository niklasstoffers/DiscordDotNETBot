using System;

namespace Hainz.InterfaceCommands 
{
    public class CommandBuilder 
    {
        private CommandType _type;
        private string[] _parameters;

        public CommandBuilder WithType(CommandType commandType) 
        {
            _type = commandType;
            return this;
        }

        public CommandBuilder WithParameters(params string[] parameters) 
        {
            _parameters = parameters;
            return this;
        }

        public Command Build() 
        {
            Command command = new Command(_type, _parameters);
            _type = 0;
            _parameters = null;
            return command;
        }
    }
}