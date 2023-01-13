using MediatR;

namespace Hainz.Data.Queries.Guild.Bans.SendDMUponBan;

public record SendDMUponBanQuery(ulong GuildId) : IRequest<bool>;