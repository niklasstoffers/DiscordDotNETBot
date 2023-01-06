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
            }
        }
    }
}