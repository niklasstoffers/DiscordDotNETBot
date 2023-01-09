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
}