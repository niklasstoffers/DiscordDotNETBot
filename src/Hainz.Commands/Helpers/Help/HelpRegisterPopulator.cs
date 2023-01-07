using System.Reflection;
using Discord.Commands;
using Hainz.Commands.Metadata;
using Hainz.Commands.Modules;

namespace Hainz.Commands.Helpers.Help;

public class HelpRegisterPopulator
{
    private readonly HelpRegister _register;
    private readonly Type[] _commandModules;

    public HelpRegisterPopulator(HelpRegister register)
    {
        _register = register;

        var assembly = Assembly.GetExecutingAssembly();
        _commandModules = assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(SocketCommandModuleBase)))
            .ToArray();
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
                AddCommandEntry(module, sectionEntry, commandNameAttribute);
        }
    }

    private void AddCommandEntry(ModuleInfo module, SectionHelpEntry sectionEntry, CommandNameAttribute commandNameAttribute)
    {
        string commandName = commandNameAttribute.Name;

        if (!string.IsNullOrEmpty(module.Group))
            commandName = $"{module.Group} {commandName}";

        var commandEntry = new CommandHelpEntry(commandName, module.Summary, module.Remarks);
        sectionEntry.Commands.Add(commandEntry);
        _register.AddCommand(commandEntry);

        foreach (var commandInfo in module.Commands)
        {
            string commandInvocationName = commandInfo.Name;

            if (!string.IsNullOrEmpty(module.Group))
                commandInvocationName = $"{module.Group} {commandInvocationName}";

            var commandInvocationEntry = new CommandInvocation(commandInvocationName);
            commandEntry.Invocations.Add(commandInvocationEntry);
            AddCommandParameters(commandInfo, commandInvocationEntry);

            var commandMethodHandle = GetMethodHandleForInvocation(module, commandInfo);
            _register.AddInvocation(commandMethodHandle, commandInvocationEntry);
        }
    }

    private static void AddCommandParameters(CommandInfo commandInfo, CommandInvocation commandInvocationEntry)
    {
        foreach (var parameter in commandInfo.Parameters)
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

    private MethodInfo GetMethodHandleForInvocation(ModuleInfo module, CommandInfo command)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        var commandModules = _commandModules
            .Where(m => 
                string.IsNullOrEmpty(module.Group) ?
                    m.Name == module.Name :
                    m.GetCustomAttribute<GroupAttribute>()?.Prefix == module.Group
            ) ?? throw new Exception($"Could not find module class for module {module.Name}");

        foreach (var commandModule in commandModules)
        {
            var methodHandle = commandModule.GetMethods()
                .SingleOrDefault(m => 
                    m.GetCustomAttribute<CommandAttribute>() != null &&
                    IsCommandMethodForInvocation(m, command)
                );

            if (methodHandle != null)
                return methodHandle;
        }

        throw new Exception($"Could not find method handle for command {command.Name}");
    }

    private static bool IsCommandMethodForInvocation(MethodInfo methodInfo, CommandInfo command)
    {
        var commandName = methodInfo.GetCustomAttribute<CommandAttribute>();
        var methodParameters = methodInfo.GetParameters();

        if (!(methodParameters.Length == command.Parameters.Count &&
              commandName?.Text == command.Name))
        {
            return false;
        }

        for (int i = 0; i < methodParameters.Length; i++)
        {
            if (methodParameters[i].ParameterType != command.Parameters[i].Type)
                return false;
        }

        return true;
    }
}