using Hainz.InterfaceCommands.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.InterfaceCommands
{
    public class CommandParser
    {
        private CommandTokenizer _tokenizer;

        public CommandParser()
        {
            _tokenizer = new CommandTokenizer();
        }

        public Command ParseCommand(string input)
        {
            var tokenList = _tokenizer.Tokenize(input);
            var tokenNode = tokenList?.First;

            StateMachine stateMachine = new StateMachine();
            while(tokenNode != null)
            {
                stateMachine.Feed(tokenNode.Value);
                tokenNode = tokenNode.Next;
            }

            if (!stateMachine.RanSuccessfull) return null;

            var commandType = CommandTokenToTypeMap.GetCommandType(stateMachine.Category, stateMachine.Name);
            if (commandType == null)
                return null;

            return new Command(commandType.Value, stateMachine.Parameters.ToArray());
        }

        private class StateMachine
        {
            private int _state = 0;
            private bool _ran = false;
            private bool _ranSuccessfull = false;

            public CommandCategory Category { get; private set; }
            public CommandName Name { get; private set; }
            public List<string> Parameters { get; private set; }
            public bool RanSuccessfull { get => _ranSuccessfull; private set => (_ran, _ranSuccessfull) = (true, value); }

            public StateMachine()
            {
                Parameters = new List<string>();
            }

            public void Feed(CommandTokenBase tokenBase)
            {
                if (_ran && !_ranSuccessfull) return;

                object token = tokenBase;
                switch (_state)
                {
                    case 0:
                        _state = 1;
                        if (token is CommandCategoryToken commandCategoryToken)
                        {
                            Category = commandCategoryToken.CommandCategory;
                            break;
                        }
                        else if (token is CommandNameToken)
                        {
                            Category = CommandCategory.None;
                            goto case 1;
                        }
                        RanSuccessfull = false;
                        break;
                    case 1:
                        if (token is CommandNameToken commandNameToken)
                        {
                            Name = commandNameToken.CommandName;
                            _state = 2;
                            RanSuccessfull = true;
                            break;
                        }
                        RanSuccessfull = false;
                        break;
                    case 2:
                        if (token is CommandParameterToken commandParameterToken)
                        {
                            Parameters.Add(commandParameterToken.Value);
                            break;
                        }
                        RanSuccessfull = false;
                        break;
                }
            }
        }
    }
}
