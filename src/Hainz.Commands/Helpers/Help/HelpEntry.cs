namespace Hainz.Commands.Helpers.Help;

public abstract class HelpEntry
{
    public string Name { get; init; }

    public HelpEntry(string name)
    {
        Name = name;
    }
}