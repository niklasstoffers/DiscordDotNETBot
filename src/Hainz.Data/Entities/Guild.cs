namespace Hainz.Data.Entities;

public class Guild : BaseEntity
{
    public ulong DiscordGuildId { get; set; }
    public IEnumerable<GuildSetting> GuildSettings { get; set; } = null!;
}