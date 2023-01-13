namespace Hainz.Data.Entities;

public partial class Guild : BaseEntity
{
    public ulong DiscordGuildId { get; set; }
    public IEnumerable<GuildSetting> GuildSettings { get; set; } = null!;
}