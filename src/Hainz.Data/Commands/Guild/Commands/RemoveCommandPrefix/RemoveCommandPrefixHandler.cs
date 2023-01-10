using Hainz.Data.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hainz.Data.Commands.Guild.Commands.RemoveCommandPrefix;

public class RemoveCommandPrefixHandler : IRequestHandler<RemoveCommandPrefixCommand, RemoveCommandPrefixResult>
{
    private readonly HainzDbContext _dbContext;

    public RemoveCommandPrefixHandler(HainzDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RemoveCommandPrefixResult> Handle(RemoveCommandPrefixCommand request, CancellationToken cancellationToken)
    {
        var customPrefix = await _dbContext.GuildSettings.SingleOrDefaultAsync(
            setting => setting.Guild.DiscordGuildId == request.GuildId &&
                setting.Name == GuildSettingName.CommandPrefix,
            cancellationToken
        );

        if (customPrefix == null)
            return RemoveCommandPrefixResult.NoCustomPrefix;

        _dbContext.GuildSettings.Remove(customPrefix);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return RemoveCommandPrefixResult.Success;
    }
}