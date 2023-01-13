using MediatR;

namespace Hainz.Data.Commands.Channel.RemoveLogChannels;

public class RemoveLogChannelsHandler : IRequestHandler<RemoveLogChannelsCommand, int>
{
    private readonly HainzDbContext _dbContext;

    public RemoveLogChannelsHandler(HainzDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(RemoveLogChannelsCommand request, CancellationToken cancellationToken)
    {
        var logChannels = _dbContext.Channels.AsEnumerable().Where(channel => channel.IsLogChannel());
        var numChannels = logChannels.Count();

        foreach (var logChannel in logChannels)
            logChannel.RemoveLogChannel();
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        return numChannels;
    }
}