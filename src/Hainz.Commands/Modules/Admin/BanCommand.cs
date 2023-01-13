using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Hainz.Commands.Metadata;
using Hainz.Commands.Modules.Admin.Parameters;
using Hainz.Commands.Preconditions;
using Hainz.Core.Services.Guild;

namespace Hainz.Commands.Modules.Admin;

[OnlyInGuild]
[RequireUserPermission(GuildPermission.BanMembers)]
[RequireBotPermission(GuildPermission.BanMembers)]
[CommandName("ban")]
[Summary("bans a user from the current guild")]
[Remarks("This command cannot be invoked on oneself")]
public sealed class BanCommand : AdminCommandBase
{
    private readonly BanService _banService;

    public BanCommand(BanService banService)
    {
        _banService = banService;
    }

    [Command("ban")]
    public async Task BanAsync([NotSelfInvokable, CommandParameter("userid", "the users id")] ulong userId, BanOptionsParameter? options = null)
    {
        if (await _banService.BanAsync(Context.Guild, userId, options?.Reason, options?.PruneDays))
        {
            await ReplyAsync($"Banned user with id {userId}");
        }
        else
        {
            await ReplyAsync($"Failed to ban user with id {userId}");
        }
    }

    [Command("ban")]
    public async Task BanAsync([NotSelfInvokable, CommandParameter("user", "mention of the user to ban")] SocketGuildUser user, BanOptionsParameter? options = null)
    {
        if (await _banService.BanAsync(user, options?.Reason, options?.PruneDays))
        {
            await ReplyAsync($"Banned user {user.Mention}");
        }
        else
        {
            await ReplyAsync($"Failed to ban user {user.Mention}");
        }
    }
}