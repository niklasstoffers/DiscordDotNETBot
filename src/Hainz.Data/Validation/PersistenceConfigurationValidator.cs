using FluentValidation;
using Hainz.Data.Configuration;

namespace Hainz.Data.Validation;

public sealed class PersistenceConfigurationValidator : AbstractValidator<PersistenceConfiguration>
{
    public PersistenceConfigurationValidator()
    {
        RuleFor(config => config.Host).NotEmpty().WithMessage("Persistence hostname may not be empty");
        RuleFor(config => config.Port).GreaterThan(0).WithMessage("Persistence port must be a valid port");
        RuleFor(config => config.Database).NotEmpty().WithMessage("Database name may not be empty");
        RuleFor(config => config.Username).NotEmpty().WithMessage("Persistence username may not be empty");
        RuleFor(config => config.Password).NotEmpty().WithMessage("Persistence password may not be empty");
    }
}