namespace Hainz.Commands.Metadata;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public sealed class CommandSectionAttribute : Attribute
{
    public string SectionName { get; init; }

    public CommandSectionAttribute(string sectionName)
    {
        SectionName = sectionName;
    }
}