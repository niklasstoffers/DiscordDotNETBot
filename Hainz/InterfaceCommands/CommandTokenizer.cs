using Hainz.InterfaceCommands.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.InterfaceCommands
{
    public class CommandTokenizer
    {
        private TokenMapper _mapper;

        public CommandTokenizer()
        {
            _mapper = new TokenMapper();
        }

        public LinkedList<CommandTokenBase> Tokenize(string input)
        {
            string[] tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var list = new LinkedList<CommandTokenBase>();

            StateMachine stateMachine = new StateMachine(_mapper);
            foreach (string token in tokens)
                list.AddLast(stateMachine.Feed(token));

            if (!stateMachine.RanSuccessfull) return null;
            return list;
        }

        private class StateMachine
        {
            private int _state = 0;
            private bool _ran = false;
            private bool _ranSuccessfull = false;
            private TokenMapper _mapper;

            public bool RanSuccessfull { get => _ranSuccessfull; set => (_ran, _ranSuccessfull) = (true, value); }

            public StateMachine(TokenMapper mapper)
            {
                _mapper = mapper;
            }

            public CommandTokenBase Feed(string token)
            {
                if (_ran && !_ranSuccessfull) return null;

                switch (_state)
                {
                    case 0:
                        var category = _mapper.Map<CommandCategory>(token);
                        _state = 1;
                        if (category == null)
                            goto case 1;
                        return new CommandCategoryToken() { CommandCategory = category.Value };
                    case 1:
                        var commandName = _mapper.Map<CommandName>(token);
                        _state = 2;
                        if (commandName == null)
                        {
                            RanSuccessfull = false;
                            return null;
                        }
                        RanSuccessfull = true;
                        return new CommandNameToken() { CommandName = commandName.Value };
                    case 2:
                        var parameterToken = new CommandParameterToken() { Value = token };
                        return parameterToken;
                }

                return null;
            }
        }
    }
}
