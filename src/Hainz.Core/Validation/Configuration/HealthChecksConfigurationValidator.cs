using FluentValidation;
using Hainz.Core.Config;

namespace Hainz.Core.Validation.Configuration;

public class HealthChecksConfigurationValidator : AbstractValidator<HealthChecksConfiguration>
{
    public HealthChecksConfigurationValidator()
    {
        RuleFor(config => config.Database).SetValidator(new HealthCheckConfigurationValidator()!).When(config => config.Database != null);
        RuleFor(config => config.Redis).SetValidator(new HealthCheckConfigurationValidator()!).When(config => config.Redis != null);
    }
}