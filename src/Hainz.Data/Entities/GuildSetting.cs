namespace Hainz.Data.Entities;

public class GuildSetting : BaseEntity
{
    public Guild Guild { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
}