namespace Hainz.Commands.Metadata;

[AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
public sealed class CommandParameterAttribute : Attribute
{
    public string Name { get; init; }
    public string Description { get; init; }
    public CommandParameterType Type { get; init; }

    public CommandParameterAttribute(CommandParameterType type, string name, string description)
    {
        Type = type;
        Name = name;
        Description = description;
    }
}