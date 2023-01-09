using FluentValidation;
using Hainz.Core.Config;

namespace Hainz.Core.Validation.Configuration;

public class HealthCheckConfigurationValidator : AbstractValidator<HealthCheckConfiguration>
{
    public HealthCheckConfigurationValidator()
    {
        RuleFor(config => config.Timeout).GreaterThan(0).When(config => config.IsEnabled);
        RuleFor(config => config.InitialTimeout).GreaterThanOrEqualTo(0).When(config => config.IsEnabled);
        RuleFor(config => config.Interval).GreaterThan(0).When(config => config.IsEnabled);
        RuleFor(config => config.UnhealthyInterval).GreaterThan(0).When(config => config.IsEnabled);
    }
}