namespace Hainz.Commands.Helpers.Help;

public class NamedCommandParameter : CommandParameter
{
    public string Placeholder { get; init; }

    public NamedCommandParameter(string name, string placeholder, string description) : base(name, description)
    {
        Placeholder = placeholder;
    }
}