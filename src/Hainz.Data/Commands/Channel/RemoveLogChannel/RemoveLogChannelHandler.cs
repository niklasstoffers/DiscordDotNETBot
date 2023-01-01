using Hainz.Data.Services.Channel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hainz.Data.Commands.Channel.RemoveLogChannel;

public class RemoveLogChannelHandler : IRequestHandler<RemoveLogChannelCommand, RemoveLogChannelResult>
{
    private readonly HainzDbContext _dbContext;

    public RemoveLogChannelHandler(HainzDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RemoveLogChannelResult> Handle(RemoveLogChannelCommand request, CancellationToken cancellationToken)
    {
        var channel = await _dbContext.Channels.SingleOrDefaultAsync(
            channel => channel.DiscordChannelId == request.ChannelId,
            cancellationToken
        );

        if (channel == null || (channel.ChannelFlags & DTOs.ChannelFlags.LogChannel) == 0)
            return RemoveLogChannelResult.NotALogChannel;
        
        channel.ChannelFlags &= ~DTOs.ChannelFlags.LogChannel;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return RemoveLogChannelResult.Success;
    }
}