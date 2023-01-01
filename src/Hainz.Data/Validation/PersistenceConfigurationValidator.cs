using FluentValidation;
using Hainz.Data.Configuration;

namespace Hainz.Data.Validation;

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