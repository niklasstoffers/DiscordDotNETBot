using Hainz.Data.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hainz.Data.Queries.Guild.Bans;

public class SendDMUponBanHandler : IRequestHandler<SendDMUponBanQuery, bool>
{
    private readonly HainzDbContext _dbContext;
    private readonly ILogger<SendDMUponBanHandler> _logger;

    public SendDMUponBanHandler(HainzDbContext dbContext, ILogger<SendDMUponBanHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<bool> Handle(SendDMUponBanQuery request, CancellationToken cancellationToken)
    {
        var guildSetting = await _dbContext.GuildSettings
            .SingleOrDefaultAsync(
                setting => setting.Guild.DiscordGuildId == request.GuildId && 
                    setting.Name == GuildSettingName.SendDMUponBan,
                cancellationToken
            );

        if (guildSetting != null)
        {
            _logger.LogInformation("{settingName} setting found for guild", GuildSettingName.SendDMUponBan);
            return bool.Parse(guildSetting.Value);
        }

        _logger.LogInformation("{settingName} setting not set for guild. Using application setting instead", GuildSettingName.SendDMUponBan);
        var applicationSetting = _dbContext.ApplicationSettings
            .SingleOrDefault(setting => setting.Name == ApplicationSettingName.SendDMUponBan);

        if (applicationSetting == null) throw new Exception($"Missing application setting for {ApplicationSettingName.SendDMUponBan}");
        return bool.Parse(applicationSetting.Value);
    }
}