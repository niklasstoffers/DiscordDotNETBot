using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Hainz.Commands.Metadata;
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
    [Summary("Bans the specified user from the Discord Server using the users id")]
    [Remarks("This command cannot be invoked on oneself")]
    public async Task BanAsync([NotSelfInvokable, CommandParameter(CommandParameterType.Id, "user id", "Id of the user to ban")] ulong userId, BanOptionsParameter? options = null)
    {
        if (await _banService.BanAsync(Context.Guild, userId, options?.Reason, options?.PruneDays))
        {
            await Context.Channel.SendMessageAsync($"Banned user with id {userId}");
        }
        else
        {
            await Context.Channel.SendMessageAsync($"Failed to ban user {userId}");
        }
    }

    [Command("ban")]
    [Summary("Bans the specified user from the Discord Server using the users mention")]
    [Remarks("This command cannot be invoked on oneself")]
    public async Task BanAsync([NotSelfInvokable, CommandParameter(CommandParameterType.Mention, "user", "Mention of the user to ban")] SocketGuildUser user, BanOptionsParameter? options = null)
    {
        if (await _banService.BanAsync(user, options?.Reason, options?.PruneDays))
        {
            await Context.Channel.SendMessageAsync($"Banned user {user.Mention}");
        }
        else
        {
            await Context.Channel.SendMessageAsync($"Failed to ban user {user.Mention}");
        }
    }
}