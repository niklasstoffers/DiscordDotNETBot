using MediatR;

namespace Hainz.Data.Commands.Guild.Commands;

public record SetCommandPrefixCommand(ulong GuildId, char CommandPrefix) : IRequest;