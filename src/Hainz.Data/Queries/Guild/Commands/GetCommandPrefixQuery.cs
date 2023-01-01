using MediatR;

namespace Hainz.Data.Queries.Guild.Commands;

public record GetCommandPrefixQuery(ulong GuildId) : IRequest<char>;