using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Hainz.Commands.Modules.Admin.Parameters;
using Hainz.Commands.Preconditions;
using Hainz.Core.Services.Guild;

namespace Hainz.Commands.Modules.Admin;

[RequireUserPermission(GuildPermission.BanMembers)]
[RequireBotPermission(GuildPermission.BanMembers)]
public sealed class BanCommand : AdminCommandBase
{
    private readonly BanService _banService;

    public BanCommand(BanService banService)
    {
        _banService = banService;
    }

    [Command("ban")]
    public async Task BanAsync([NotSelfInvokable] ulong userId, BanOptionsParameter? options = null)
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
    public async Task BanAsync([NotSelfInvokable] SocketGuildUser user, BanOptionsParameter? options = null)
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