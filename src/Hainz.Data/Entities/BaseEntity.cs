namespace Hainz.Data.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; } = 0;
    public DateTime Updated { get; set; } = DateTime.UtcNow;
    public DateTime Added { get; set; } = DateTime.UtcNow;
}