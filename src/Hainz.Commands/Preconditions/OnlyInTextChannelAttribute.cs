using Discord;
using Discord.Commands;
using Hainz.Commands.Helpers;

namespace Hainz.Commands.Preconditions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class OnlyInTextChannelAttribute : PreconditionAttribute
{
    public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
    {
        return Task.FromResult(PreconditionResultBuilder.SuccessIf(context.Channel is ITextChannel, "This command can only be invoked in a text channel"));
    }
}