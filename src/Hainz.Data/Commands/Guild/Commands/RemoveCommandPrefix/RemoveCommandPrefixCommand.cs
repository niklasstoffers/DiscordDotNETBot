using MediatR;

namespace Hainz.Data.Commands.Guild.Commands.RemoveCommandPrefix;

public record RemoveCommandPrefixCommand(ulong GuildId) : IRequest<RemoveCommandPrefixResult>;