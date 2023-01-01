using MediatR;

namespace Hainz.Data.Queries.Guild.Bans;

public record SendDMUponBanQuery(ulong GuildId) : IRequest<bool>;