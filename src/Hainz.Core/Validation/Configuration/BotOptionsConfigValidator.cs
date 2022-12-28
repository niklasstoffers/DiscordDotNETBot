using FluentValidation;
using Hainz.Core.Config.BotOptions;

namespace Hainz.Core.Validation.Configuration;

public sealed class BotOptionsConfigValidator : AbstractValidator<BotOptionsConfig>
{
    public BotOptionsConfigValidator()
    {
        RuleFor(config => config.Bans)
            .NotNull();
    }
}