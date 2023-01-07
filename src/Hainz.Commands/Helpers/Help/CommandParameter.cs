namespace Hainz.Commands.Helpers.Help;

public class CommandParameter
{
    public string Name { get; init; }
    public string Description { get; init; }
    public bool IsOptional { get; init; }

    public CommandParameter(string name, string description, bool isOptional)
    {
        Name = name;
        Description = description;
        IsOptional = isOptional;
    }
}