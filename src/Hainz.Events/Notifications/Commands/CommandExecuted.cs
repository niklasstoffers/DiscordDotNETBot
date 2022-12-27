using Discord;
using Discord.Commands;
using MediatR;

namespace Hainz.Events.Notifications.Commands;

public class CommandExecuted : INotification
{
    public Optional<CommandInfo> CommandInfo { get; init; }
    public ICommandContext CommandContext { get; init; }
    public IResult Result { get; init; }

    public CommandExecuted(Optional<CommandInfo> commandInfo,
                           ICommandContext commandContext,
                           IResult result)
    {
        CommandInfo = commandInfo;
        CommandContext = commandContext;
        Result = result;
    }
}