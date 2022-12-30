using MediatR;

namespace Hainz.Data.Queries.Guilds.BanDMs;

public sealed class SendDMUponBanRequestHandler : IRequestHandler<SendDMUponBanQuery, bool>
{
    private readonly IDbContext _dbContext;

    public SendDMUponBanRequestHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> Handle(SendDMUponBanQuery request, CancellationToken cancellationToken)
    {
        var guild = _dbContext.Guilds.SingleOrDefaultAsync
        return guild?.SendDMUponBan ?? true;
    }
}