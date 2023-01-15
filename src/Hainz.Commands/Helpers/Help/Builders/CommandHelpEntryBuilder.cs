using System.Text;
using Discord;
using Discord.Commands;

namespace Hainz.Commands.Helpers.Help.Builders;

public class CommandHelpEntryBuilder : HelpEntryBuilderBase<CommandHelpEntry>
{
    private const string _OPTIONAL_POSTFIX = "?";

    private readonly CommandPrefixResolver _prefixResolver;

    public CommandHelpEntryBuilder(CommandPrefixResolver prefixResolver)
    {
        _prefixResolver = prefixResolver;
    }

    protected override async Task Fill(EmbedBuilder builder, SocketCommandContext context, CommandHelpEntry entry)
    {
        var contentBuilder = new StringBuilder();

        contentBuilder.AppendLine(Format.Underline(entry.Description));
        contentBuilder.AppendLine();

        foreach (var invocation in entry.Invocations)
        {
            contentBuilder.AppendLine(await BuildInvocation(invocation, context));

            if (entry.Invocations.Count > 1)
                contentBuilder.AppendLine();
        }

        if (entry.Invocations.Any(invocation => invocation.Parameters.Any(parameter => parameter.IsOptional)))
            contentBuilder.AppendLine(Format.Underline($"Parameters marked with {_OPTIONAL_POSTFIX} are optional."));

        if (!string.IsNullOrEmpty(entry.Remarks))
        {
            contentBuilder.AppendLine();
            contentBuilder.AppendLine(Format.Italics(entry.Remarks));
        }

        builder.Description = contentBuilder.ToString();
    }

    public async Task<string> BuildInvocation(CommandInvocation invocation, SocketCommandContext context, bool invocationOnly = false)
    {
        char prefix = await _prefixResolver.GetPrefix(context.Channel);
        string parameterList = BuildParameterList(invocation);
        var invocationBuilder = new StringBuilder();

        if (invocationOnly)
            return Format.Code($"{prefix}{invocation.Name} {parameterList}".TrimEnd());

        if (invocation.Parameters.Count > 0)
        {
            invocationBuilder.AppendLine(Format.Bold("Command:"));
            invocationBuilder.AppendLine();
        }
        else
            invocationBuilder.Append(Format.Bold("Command: "));

        invocationBuilder.AppendLine($"{Format.Code($"{prefix}{invocation.Name} {parameterList}".TrimEnd())}");

        if (!(invocation.Summary == null && invocation.Remarks == null))
        {
            invocationBuilder.AppendLine();

            if (invocation.Summary != null) 
                invocationBuilder.AppendLine($"{Format.Bold("Summary:")} {invocation.Summary}");
            if (invocation.Remarks != null)
                invocationBuilder.AppendLine($"{Format.Bold("Remarks:")} {invocation.Remarks}");
        }

        if (invocation.Parameters.Count > 0)
        {
            invocationBuilder.AppendLine();
            invocationBuilder.AppendLine(BuildParameterDescriptions(invocation));
        }

        if (invocation.Parameters.Count > 0 || !(invocation.Summary == null && invocation.Remarks == null))
            return Format.Quote(invocationBuilder.ToString());
        return invocationBuilder.ToString();
    }

    private static string BuildParameterList(CommandInvocation invocation) =>
        string.Join(" ", invocation.Parameters.Select(parameter => BuildParameter(parameter)));

    private static string BuildParameter(CommandParameter parameter)
    {
        string parameterPostfix = parameter.IsOptional ? _OPTIONAL_POSTFIX : "";
        if (parameter is NamedCommandParameter namedParameter) 
            return $"{namedParameter.Name}{parameterPostfix}: <{namedParameter.Placeholder}>";
        else
            return $"<{parameter.Name}{parameterPostfix}>";
    }

    private static string BuildParameterDescriptions(CommandInvocation invocation) =>
        string.Join("\n", invocation.Parameters.Select(parameter => BuildParameterDescription(parameter)));

    private static string BuildParameterDescription(CommandParameter parameter)
    {
        string parameterPostfix = parameter.IsOptional ? _OPTIONAL_POSTFIX : "";
        string parameterPlaceholder = parameter.Name;

        if (parameter is NamedCommandParameter namedParameter)
            parameterPlaceholder = namedParameter.Placeholder;
        
        return $"{Format.Bold($"{parameterPlaceholder}{parameterPostfix}")} - {parameter.Description}";
    }
}