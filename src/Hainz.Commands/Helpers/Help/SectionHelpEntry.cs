namespace Hainz.Commands.Helpers.Help;

public class SectionHelpEntry : HelpEntry
{
    public string Description { get; init; }
    public List<CommandHelpEntry> Commands { get; init; }

    public SectionHelpEntry(string name, string description) : base(name)
    {
        Description = description;
        Commands = new();
    }
}