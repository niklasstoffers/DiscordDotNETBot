namespace Hainz.Commands.Metadata;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class NamedCommandParameterAttribute : Attribute
{
    public string Placeholder { get; init; }
    public string Description { get; init; }

    public NamedCommandParameterAttribute(string placeholder, string description)
    {
        Placeholder = placeholder;
        Description = description;
    }
}