using System.Linq.Expressions;
using System.Reflection;
using Discord.Commands;
using Hainz.Commands.Helpers.Help.Builders;
using Hainz.Commands.Modules;

namespace Hainz.Commands.Helpers.Help;

public class CommandInvocationResolver
{
    private readonly HelpRegister _helpRegister;
    private readonly CommandHelpEntryBuilder _commandHelpBuilder;

    public CommandInvocationResolver(HelpRegister helpRegister, CommandHelpEntryBuilder commandHelpBuilder)
    {
        _helpRegister = helpRegister;
        _commandHelpBuilder = commandHelpBuilder;
    }

    public async Task<string> GetInvocation<T>(Expression<Func<T, Delegate>> commandMethodSelectorExpression, SocketCommandContext context) where T : SocketCommandModuleBase
    {
        if (commandMethodSelectorExpression.Body is UnaryExpression createDelegateConversionExpression &&
            createDelegateConversionExpression.NodeType == ExpressionType.Convert &&
            createDelegateConversionExpression.Operand is MethodCallExpression commandMethodCallExpression &&
            commandMethodCallExpression.Object is ConstantExpression commandMethodHandleExpression &&
            commandMethodHandleExpression.Value is MethodInfo commandMethodHandle &&
            _helpRegister.GetInvocationByMethod(commandMethodHandle) is CommandInvocation commandInvocation)
        {
            return await _commandHelpBuilder.BuildInvocation(commandInvocation, context, true);
        }

        throw new ArgumentException("Invalid command method selector expression");
    }
}