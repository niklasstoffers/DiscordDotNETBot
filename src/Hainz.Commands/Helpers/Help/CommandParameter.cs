namespace Hainz.Commands.Helpers.Help;

public class CommandParameter
{
    public string Name { get; init; }
    public string Description { get; init; }

    public CommandParameter(string name, string description)
    {
        Name = name;
        Description = description;
    }
}