namespace Hainz.Commands.Helpers.Help;

public class CommandInvocation
{
    public string Name { get; init; }
    public List<CommandParameter> Parameters { get; init; }

    public CommandInvocation(string name)
    {
        Name = name;
        Parameters = new();
    }
}