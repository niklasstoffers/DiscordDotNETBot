using Hainz.Data.DTOs;
using Hainz.Data.Entities;

namespace Hainz.Data;

public class Channel : BaseEntity
{
    public ulong DiscordChannelId { get; set; }
    public ChannelFlags ChannelFlags { get; set; }
}