namespace Hainz.Commands.Metadata;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class CommandNameAttribute : Attribute
{
    public string Name { get; init; }

    public CommandNameAttribute(string name)
    {
        Name = name;
    }
}