using FluentValidation;
using Hainz.Commands.Config;
using Hainz.Commands.Helpers;

namespace Hainz.Commands.Validation;

public sealed class CommandsConfigValidator : AbstractValidator<CommandsConfig>
{
    public CommandsConfigValidator()
    {
        RuleFor(config => config.FallbackPrefix)
            .Must(prefix => CommandPrefixValidator.IsValidPrefix(prefix));
    }
}