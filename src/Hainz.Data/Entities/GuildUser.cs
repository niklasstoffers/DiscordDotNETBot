namespace Hainz.Data.Entities;

public sealed class GuildUser : BaseEntity
{
    public ulong GuildUserId { get; set; }
    public bool HasElevatedPrivileges { get; set; }
    public IEnumerable<Guild> Guilds { get; set; } = null!;
}