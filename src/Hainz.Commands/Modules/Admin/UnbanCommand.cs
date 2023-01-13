using Discord;
using Discord.Commands;
using Hainz.Commands.Metadata;
using Hainz.Commands.Preconditions;
using Hainz.Core.Services.Guild;

namespace Hainz.Commands.Modules.Admin;

[OnlyInGuild]
[RequireUserPermission(GuildPermission.BanMembers)]
[RequireBotPermission(GuildPermission.BanMembers)]
[CommandName("unban")]
[Summary("unbans a user from the guild")]
public sealed class UnbanCommand : AdminCommandBase
{
    private readonly BanService _banService;

    public UnbanCommand(BanService banService)
    {
        _banService = banService;
    }

    [Command("unban")]
    public async Task UnbanAsync([CommandParameter("userid", "id of the user to unban")] ulong userId)
    {
        if (await _banService.UnbanAsync(Context.Guild, userId))
        {
            await ReplyAsync($"Unbanned user with id {userId}");
        }
        else
        {
            await ReplyAsync($"Failed to unban user with id {userId}");
        }
    }
}