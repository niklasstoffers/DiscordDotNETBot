using Discord;
using Discord.Commands;
using Hainz.Commands.Metadata;
using Hainz.Commands.Preconditions;

namespace Hainz.Commands.Modules.Misc;

[OnlyInGuild]
[OnlyInTextChannel]
[RequireBotPermission(GuildPermission.ManageMessages)]
[RequireUserPermission(GuildPermission.ManageMessages)]
[CommandName("clear")]
[Summary("clears messages")]
public class ClearCommand : MiscCommandBase
{
    private const int _CLEAR_MAX = 200;
    private const int _CLEAR_RESPONSE_TIMEOUT = 5000;

    [Command("clear")]
    public async Task ClearAsync([CommandParameter("count", "number of messages to clear.")] int count = 20)
    {
        if (count < 0)
            await ReplyAsync("Cannot remove a negative number of messages.");
        else if (count > _CLEAR_MAX)
            await ReplyAsync($"Cannot remove more than {_CLEAR_MAX} messages at once.");
        else if (Context.Channel is ITextChannel textChannel)
        {
            var messages = await Context.Channel.GetMessagesAsync(count).FlattenAsync();
            await textChannel.DeleteMessagesAsync(messages);
            var message = await ReplyAsync($"Cleared {count} messages.");
            await Task.Delay(_CLEAR_RESPONSE_TIMEOUT);
            await message.DeleteAsync();
        }
    }
}