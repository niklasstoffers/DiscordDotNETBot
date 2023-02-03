using Discord;
using Discord.Commands;
using MediatR;

namespace Hainz.Events.Notifications.Commands;

public record CommandExecuted(Optional<CommandInfo> CommandInfo, ICommandContext CommandContext, IResult Result) : INotification;