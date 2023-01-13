using MediatR;

namespace Hainz.Data.Queries.Guild.Commands.GetCommandPrefix;

public record GetCommandPrefixQuery(ulong GuildId) : IRequest<char>;