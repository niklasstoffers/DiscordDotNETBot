namespace Hainz.Commands.Metadata;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public sealed class CommandSectionAttribute : Attribute
{
    public string Name { get; init; }
    public string Description { get; init; }

    public CommandSectionAttribute(string name, string description)
    {
        Name = name;
        Description = description;
    }
}