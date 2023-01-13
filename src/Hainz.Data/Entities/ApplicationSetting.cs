namespace Hainz.Data.Entities;

public partial class ApplicationSetting : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
}