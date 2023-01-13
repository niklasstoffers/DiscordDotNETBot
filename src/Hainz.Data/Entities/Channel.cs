using Hainz.Data.DTOs;

namespace Hainz.Data.Entities;

public partial class Channel : BaseEntity
{
    public ulong DiscordChannelId { get; set; }
    public ChannelFlags ChannelFlags { get; set; }
}