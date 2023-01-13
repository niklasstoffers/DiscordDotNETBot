using System.Text;
using Discord;
using Discord.WebSocket;

namespace Hainz.Commands.Extensions;

public static class EmbedBuilderExtensions
{
    public static EmbedBuilder WithUser(this EmbedBuilder builder, IUser user)
    {
        builder.WithFooter(footerBuilder =>
        {
            footerBuilder.Text = user.Username;
            footerBuilder.IconUrl = user.GetAvatarUrl();
        });

        return builder;
    }

    public static EmbedBuilder WithContent(this EmbedBuilder builder, Action<StringBuilder> action)
    {
        var stringBuilder = new StringBuilder();
        action(stringBuilder);
        builder.Description = stringBuilder.ToString();

        return builder;
    }

    public static async Task<EmbedBuilder> WithContent(this EmbedBuilder builder, Func<StringBuilder, Task> func)
    {
        var stringBuilder = new StringBuilder();
        await func(stringBuilder);
        builder.Description = stringBuilder.ToString();

        return builder;
    }
}