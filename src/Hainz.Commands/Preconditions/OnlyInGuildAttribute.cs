using Discord.Commands;
using Hainz.Commands.Helpers;

namespace Hainz.Commands.Preconditions;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class OnlyInGuildAttribute : PreconditionAttribute
{
    public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
    {
        return Task.FromResult(PreconditionResultBuilder.SuccessIf(context.Guild != null, "This command is only invokable in a guild"));
    }
}