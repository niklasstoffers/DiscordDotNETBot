using System.Reflection;
using FuzzySharp;

namespace Hainz.Commands.Helpers.Help;

public class HelpRegister
{
    private const int MIN_MATCH_SCORE = 85;

    public List<SectionHelpEntry> Sections { get; init; }
    public Dictionary<string, HelpEntry> Entries { get; init; }
    public Dictionary<MethodInfo, CommandInvocation> CommandInvocationMap { get; init; }

    public HelpRegister()
    {
        Sections = new();
        Entries = new();
        CommandInvocationMap = new Dictionary<MethodInfo, CommandInvocation>();
    }

    public HelpEntry? GetEntry(string search, out bool wasFuzzyMatch)
    {
        wasFuzzyMatch = false;
        if (Entries.ContainsKey(search))
            return Entries[search];

        var closestMatch = Process.ExtractOne(search, Entries.Keys, cutoff: MIN_MATCH_SCORE);
        if (closestMatch != null)
        {
            wasFuzzyMatch = true;
            return Entries[closestMatch.Value];
        }

        return null;
    }

    public void AddSection(SectionHelpEntry section)
    {
        Sections.Add(section);
        Entries.Add(section.Name, section);
    }

    public void AddCommand(CommandHelpEntry command)
    {
        Entries.Add(command.Name, command);
    }

    public void AddInvocation(MethodInfo methodInfo, CommandInvocation invocation) => CommandInvocationMap.Add(methodInfo, invocation);
    public CommandInvocation? GetInvocationByMethod(MethodInfo methodInfo) => CommandInvocationMap.GetValueOrDefault(methodInfo);
}
