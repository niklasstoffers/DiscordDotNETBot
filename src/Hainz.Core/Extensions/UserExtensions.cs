using Discord;

namespace Hainz.Core.Extensions;

public static class UserExtensions
{
    public static string GetUsernameWithDiscriminator(this IUser user) => $"{user.Username}#{user.Discriminator}";
}