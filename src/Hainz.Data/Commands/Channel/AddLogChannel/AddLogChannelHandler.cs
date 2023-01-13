using Hainz.Data.Services.Channel;
using MediatR;

namespace Hainz.Data.Commands.Channel.AddLogChannel;

public class AddLogChannelHandler : IRequestHandler<AddLogChannelCommand, AddLogChannelResult>
{
    private readonly HainzDbContext _dbContext;
    private readonly ChannelService _channelService;

    public AddLogChannelHandler(HainzDbContext dbContext, ChannelService channelService)
    {
        _dbContext = dbContext;
        _channelService = channelService;
    }

    public async Task<AddLogChannelResult> Handle(AddLogChannelCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelService.GetOrCreateByDiscordId(request.ChannelId);
        
        if (channel.IsLogChannel())
            return AddLogChannelResult.AlreadyALogChannel;

        channel.MakeLogChannel();
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return AddLogChannelResult.Success;
    }
}