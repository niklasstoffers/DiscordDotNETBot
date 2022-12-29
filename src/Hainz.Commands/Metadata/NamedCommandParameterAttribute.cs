namespace Hainz.Commands.Metadata;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class NamedCommandParameterAttribute : Attribute
{
    public CommandParameterType Type { get; init; }
    public string Placeholder { get; init; }
    public string Description { get; init; }

    public NamedCommandParameterAttribute(CommandParameterType type, string placeholder, string description)
    {
        Type = type;
        Placeholder = placeholder;
        Description = description;
    }
}