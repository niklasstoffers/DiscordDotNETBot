using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Hainz.Commands.Embeds;
using Hainz.Commands.Extensions;
using Hainz.Commands.Metadata;
using Hainz.Data.Queries.Channels.LogChannels.GetLogChannels;
using MediatR;

namespace Hainz.Commands.Modules.Bot.LogChannel;

[CommandName("list")]
[Summary("list of all log channels")]
public class ListLogChannelCommand : LogChannelCommandBase
{
    private readonly IMediator _mediator;
    private readonly DefaultEmbedTemplateBuilder _embedTemplateBuilder;

    public ListLogChannelCommand(IMediator mediator, DefaultEmbedTemplateBuilder embedTemplateBuilder)
    {
        _mediator = mediator;
        _embedTemplateBuilder = embedTemplateBuilder;
    }

    [Command("list")]
    public async Task ListLogChannelAsync()
    {
        var logChannels = await _mediator.Send(new GetLogChannelsQuery());
        var embedBuilder = _embedTemplateBuilder.Build();
        
        await embedBuilder.WithContent(async builder =>
        {
            if (logChannels.Any())
            {
                builder.AppendLine(Format.Underline("List of log channels:"));
                builder.AppendLine();
            }
            else 
                builder.AppendLine("No log channels found");

            foreach (var logChannel in logChannels)
            {
                var channel = await Context.Client.GetChannelAsync(logChannel.ChannelId);
                string channelName;

                if (channel is SocketGuildChannel guildChannel && guildChannel.Guild.Id == Context.Guild?.Id)
                    channelName = channel.Name;
                else if (channel != null)
                    channelName = "Not in this guild";
                else
                    channelName = "Not found";

                builder.AppendLine($"{Format.Italics(logChannel.ChannelId.ToString())} - {Format.Bold(channelName)}");
            }
        });

        await ReplyAsync(embed: embedBuilder.Build());
    }
}