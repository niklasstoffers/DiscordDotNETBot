namespace Hainz.Data.Entities;

public sealed class Guild : BaseEntity 
{
    public ulong GuildId { get; set; }
    public bool SendDMUponBan { get; set; }
    public IEnumerable<GuildChannel> Channels { get; set; } = null!;
    public IEnumerable<GuildUser> Users { get; set; } = null!;
}