using Hainz.Data.DTOs;
using Hainz.Data.Entities;

namespace Hainz.Data.Entities;

public class Channel : BaseEntity
{
    public ulong DiscordChannelId { get; set; }
    public ChannelFlags ChannelFlags { get; set; }
}