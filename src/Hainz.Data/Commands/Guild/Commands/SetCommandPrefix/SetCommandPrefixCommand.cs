using MediatR;

namespace Hainz.Data.Commands.Guild.Commands.SetCommandPrefix;

public record SetCommandPrefixCommand(ulong GuildId, char CommandPrefix) : IRequest;