namespace Hainz.Commands.Helpers.Help;

public class CommandHelpEntry : HelpEntry
{
    public string Description { get; init; }
    public string? Remarks { get; init; }
    public List<CommandInvocation> Invocations { get; init; }

    public CommandHelpEntry(string name, string description, string? remarks) : base(name)
    {
        Description = description;
        Remarks = remarks;
        Invocations = new();
    }
}