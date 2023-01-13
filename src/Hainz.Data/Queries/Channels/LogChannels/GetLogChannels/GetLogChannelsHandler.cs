using Hainz.Data.DTOs.Discord;
using MediatR;

namespace Hainz.Data.Queries.Channels.LogChannels.GetLogChannels;

public class GetLogChannelsHandler : IRequestHandler<GetLogChannelsQuery, IEnumerable<ChannelDTO>>
{
    private readonly HainzDbContext _dbContext;

    public GetLogChannelsHandler(HainzDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<IEnumerable<ChannelDTO>> Handle(GetLogChannelsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_dbContext.Channels.AsEnumerable().Where(channel => channel.IsLogChannel())
            .Select(x => new ChannelDTO(x.DiscordChannelId))
            .AsEnumerable());
    }
}