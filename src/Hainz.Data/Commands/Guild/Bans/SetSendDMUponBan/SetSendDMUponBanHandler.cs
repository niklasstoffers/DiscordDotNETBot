using Hainz.Data.DTOs;
using Hainz.Data.Entities;
using Hainz.Data.Services.Guild;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hainz.Data.Commands.Guild.Bans.SetSendDMUponBan;

public class SetSendDMUponBanHandler : IRequestHandler<SetSendDMUponBanCommand, Unit>
{
    private readonly HainzDbContext _dbContext;
    private readonly GuildService _guildService;

    public SetSendDMUponBanHandler(HainzDbContext dbContext, GuildService guildService)
    {
        _dbContext = dbContext;
        _guildService = guildService;
    }

    public async Task<Unit> Handle(SetSendDMUponBanCommand request, CancellationToken cancellationToken)
    {
        var guild = await _guildService.GetOrCreateByDiscordId(request.GuildId);
        var setting = await _dbContext.GuildSettings.SingleOrDefaultAsync(
            setting => setting.Guild.DiscordGuildId == request.GuildId &&
                setting.Name == GuildSettingName.SendDMUponBan,
            cancellationToken
        );

        if (setting == null)
        {
            setting = new GuildSetting()
            {
                Name = GuildSettingName.SendDMUponBan,
                Guild = guild
            };

            _dbContext.GuildSettings.Add(setting);
        }

        setting.Value = request.IsEnabled.ToString();
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}