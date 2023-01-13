using Hainz.Data.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hainz.Data.Queries.Guild.Commands.GetCommandPrefix;

public class GetCommandPrefixHandler : IRequestHandler<GetCommandPrefixQuery, char>
{
    private readonly HainzDbContext _dbContext;

    public GetCommandPrefixHandler(HainzDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<char> Handle(GetCommandPrefixQuery request, CancellationToken cancellationToken)
    {
        var guildPrefixSetting = await _dbContext.GuildSettings.SingleOrDefaultAsync(
            setting => setting.Guild.DiscordGuildId == request.GuildId &&
                setting.Name == GuildSettingName.CommandPrefix,
            cancellationToken
        );

        if (guildPrefixSetting != null)
            return guildPrefixSetting.Value[0];
        
        var applicationPrefixSetting = _dbContext.ApplicationSettings
            .SingleOrDefault(setting => setting.Name == ApplicationSettingName.CommandPrefix);
        
        if (applicationPrefixSetting == null || string.IsNullOrEmpty(applicationPrefixSetting.Value)) 
            throw new Exception($"Missing setting for {ApplicationSettingName.CommandPrefix}");

        return applicationPrefixSetting.Value[0];
    }
}