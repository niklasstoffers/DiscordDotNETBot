using MediatR;

namespace Hainz.Data.Queries.Guilds.BanDMs; 

public record SendDMUponBanQuery(ulong GuildId) : IRequest<bool>;