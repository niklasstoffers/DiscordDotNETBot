namespace Hainz.Commands.Metadata;

[AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
public sealed class CommandParameterAttribute : Attribute
{
    public string Name { get; init; }
    public string Description { get; init; }

    public CommandParameterAttribute(string name, string description)
    {
        Name = name;
        Description = description;
    }
}