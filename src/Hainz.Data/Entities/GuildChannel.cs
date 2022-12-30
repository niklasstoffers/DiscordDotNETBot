namespace Hainz.Data.Entities;

public sealed class GuildChannel : BaseEntity
{
    public ulong ChannelId { get; set; }
    public Guild Guild { get; set; } = null!;
    public GuildChannelType ChannelType { get; set; }
}