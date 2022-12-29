using FluentValidation;
using Hainz.Persistence.Configuration;

namespace Hainz.Persistence.Validation;

public sealed class PersistenceConfigurationValidator : AbstractValidator<PersistenceConfiguration>
{
    public PersistenceConfigurationValidator()
    {
        RuleFor(config => config.Host).NotEmpty();
        RuleFor(config => config.Database).NotEmpty();
        RuleFor(config => config.Username).NotEmpty();
        RuleFor(config => config.Password).NotEmpty();
    }
}