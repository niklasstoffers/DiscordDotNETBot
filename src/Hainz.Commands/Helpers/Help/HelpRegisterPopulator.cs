using System.Reflection;
using Discord.Commands;
using Hainz.Commands.Metadata;

namespace Hainz.Commands.Helpers.Help;

public class HelpRegisterPopulator
{
    private readonly HelpRegister _register;

    public HelpRegisterPopulator(HelpRegister register)
    {
        _register = register;
    }

    public void Populate(CommandService commandService)
    {
        Dictionary<string, SectionHelpEntry> sectionHelpMap = new();

        foreach (var module in commandService.Modules)
        {
            var sectionAttribute = (CommandSectionAttribute)module.Attributes.Single(attr => attr is CommandSectionAttribute);
            SectionHelpEntry sectionEntry;

            if (!sectionHelpMap.ContainsKey(sectionAttribute.Name))
            {
                sectionEntry = new SectionHelpEntry(sectionAttribute.Name, sectionAttribute.Description);
                sectionHelpMap.Add(sectionEntry.Name, sectionEntry);
                _register.AddSection(sectionEntry);
            }
            else
            {
                sectionEntry = sectionHelpMap[sectionAttribute.Name];
            }

            if (module.Attributes.SingleOrDefault(attr => attr is CommandNameAttribute) is CommandNameAttribute commandNameAttribute)
            {
                string commandName = commandNameAttribute.Name;

                if (!string.IsNullOrEmpty(module.Group))
                    commandName = $"{module.Group} {commandName}";

                var commandEntry = new CommandHelpEntry(commandName, module.Summary, module.Remarks);
                sectionEntry.Commands.Add(commandEntry);
                _register.AddCommand(commandEntry);

                foreach (var commandInvocation in module.Commands)
                {
                    string commandInvocationName = commandInvocation.Name;

                    if (!string.IsNullOrEmpty(module.Group))
                        commandInvocationName = $"{module.Group} {commandInvocationName}";

                    var commandInvocationEntry = new CommandInvocation(commandInvocationName);
                    commandEntry.Invocations.Add(commandInvocationEntry);

                    foreach (var parameter in commandInvocation.Parameters)
                    {
                        var isNamedParameter = parameter.Type.GetCustomAttribute<NamedArgumentTypeAttribute>() != null;
                        if (isNamedParameter)
                        {
                            foreach (var parameterProperty in parameter.Type.GetProperties())
                            {
                                var namedParameterAttribute = parameterProperty.GetCustomAttribute<NamedCommandParameterAttribute>()!;
                                var namedParameterEntry = new NamedCommandParameter(parameterProperty.Name.ToLower(), namedParameterAttribute.Placeholder, namedParameterAttribute.Description);
                                commandInvocationEntry.Parameters.Add(namedParameterEntry);
                            }
                        }
                        else
                        {
                            var parameterAttribute = (CommandParameterAttribute)parameter.Attributes.Single(attr => attr is CommandParameterAttribute);
                            var parameterEntry = new CommandParameter(parameterAttribute.Name, parameterAttribute.Description, parameter.IsOptional);
                            commandInvocationEntry.Parameters.Add(parameterEntry);
                        }
                    }
                }
            }
        }
    }
}