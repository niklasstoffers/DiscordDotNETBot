using Hainz.Data.DTOs;
using Hainz.Data.Services.Guild;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hainz.Data.Commands.Guild.Commands.SetCommandPrefix;

public class SetCommandPrefixHandler : IRequestHandler<SetCommandPrefixCommand, Unit>
{
    private readonly HainzDbContext _dbContext;
    private readonly GuildService _guildService;

    public SetCommandPrefixHandler(HainzDbContext dbContext, GuildService guildService)
    {
        _dbContext = dbContext;
        _guildService = guildService;
    }

    public async Task<Unit> Handle(SetCommandPrefixCommand request, CancellationToken cancellationToken)
    {
        var guild = await _guildService.GetOrCreateByDiscordId(request.GuildId);
        var prefixSetting = await _dbContext.GuildSettings.SingleOrDefaultAsync(
            setting => setting.Guild.DiscordGuildId == request.GuildId &&
                setting.Name == GuildSettingName.CommandPrefix,
            cancellationToken
        );

        if (prefixSetting == null)
        {
            prefixSetting = new Entities.GuildSetting()
            {
                Guild = guild,
                Name = GuildSettingName.CommandPrefix
            };

            _dbContext.GuildSettings.Add(prefixSetting);
        }

        prefixSetting.Value = request.CommandPrefix.ToString();
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}