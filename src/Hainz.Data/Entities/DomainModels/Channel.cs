namespace Hainz.Data.Entities;

public partial class Channel
{
    public void MakeLogChannel() => ChannelFlags |= DTOs.ChannelFlags.LogChannel;
    public void RemoveLogChannel() => ChannelFlags &= ~DTOs.ChannelFlags.LogChannel;
    public bool IsLogChannel() => (ChannelFlags & DTOs.ChannelFlags.LogChannel) != 0;
}