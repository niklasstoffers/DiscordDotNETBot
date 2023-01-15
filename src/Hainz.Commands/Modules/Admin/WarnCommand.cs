using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Hainz.Commands.Metadata;
using Hainz.Commands.Preconditions;
using Hainz.Core.Extensions;
using Hainz.Core.Services.Messages;

namespace Hainz.Commands.Modules.Admin;

[OnlyInGuild]
[RequireUserPermission(GuildPermission.ModerateMembers)]
[CommandName("warn")]
[Summary("warns a user")]
public class WarnCommand : AdminCommandBase
{
    private readonly DMService _dmService;

    public WarnCommand(DMService dmService)
    {
        _dmService = dmService;
    }

    [Command("warn")]
    public async Task WarnAsync(
        [NotSelfInvokable, CommandParameter("user", "the user to warn")] SocketGuildUser user,
        [CommandParameter("reason", "reason why user is getting warned")] string? reason = null)
    {
        var embedBuilder = new EmbedBuilder();
        string warnMessage = $"You received a warning from {Context.User.Mention} in Guild \"{Context.Guild.Name}\". ";

        if (reason != null)
            warnMessage += $"Reason: \"{reason}\"";

        if (await _dmService.SendDMAsync(user.Id, warnMessage))
        {
            embedBuilder
                .WithTitle($"Warned user {user.GetUsernameWithDiscriminator()}")
                .WithThumbnailUrl(user.GetAvatarUrl());
            
            embedBuilder.Description = $"{Format.Bold("Reason:")} {reason ?? "Unspecified"}";
            await ReplyAsync(embed: embedBuilder.Build());
        }
        else
        {
            await ReplyAsync("Error while trying to warn user");
        }
    }
}