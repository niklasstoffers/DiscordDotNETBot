using Discord;
using Discord.Commands;
using Hainz.Core.Services.User;

namespace Hainz.Commands.Modules.Admin;

[RequireUserPermission(GuildPermission.BanMembers)]
[RequireBotPermission(GuildPermission.BanMembers)]
public sealed class UnbanCommand : AdminCommandBase
{
    private readonly BanService _banService;

    public UnbanCommand(BanService banService)
    {
        _banService = banService;
    }

    [Command("unban")]
    public async Task UnbanAsync(ulong userId)
    {
        if (await _banService.UnbanAsync(Context.Guild, userId))
        {
            await Context.Channel.SendMessageAsync($"Unbanned user with id {userId}");
        }
        else
        {
            await Context.Channel.SendMessageAsync($"Failed to unban user with id {userId}");
        }
    }
}