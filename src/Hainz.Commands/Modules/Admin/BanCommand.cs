using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Hainz.Core.Services.User;

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
    public async Task BanAsync(ulong userId)
    {
        if (await _banService.BanAsync(Context.Guild, userId))
        {
            await Context.Channel.SendMessageAsync($"Banned user with id {userId}");
        }
        else
        {
            await Context.Channel.SendMessageAsync($"Failed to ban user {userId}");
        }
    }

    [Command("ban")]
    public async Task BanAsync(SocketGuildUser user)
    {
        if (await _banService.BanAsync(user))
        {
            await Context.Channel.SendMessageAsync($"Banned user {user.Mention}");
        }
        else
        {
            await Context.Channel.SendMessageAsync($"Failed to ban user {user.Mention}");
        }
    }
}