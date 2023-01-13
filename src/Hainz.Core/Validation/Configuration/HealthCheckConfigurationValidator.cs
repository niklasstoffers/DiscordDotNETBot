using FluentValidation;
using Hainz.Core.Config;

namespace Hainz.Core.Validation.Configuration;

public class HealthCheckConfigurationValidator : AbstractValidator<HealthCheckConfiguration>
{
    public HealthCheckConfigurationValidator()
    {
        RuleFor(config => config.Timeout).GreaterThan(0).When(config => config.IsEnabled).WithMessage("Timeout must be greater than 0");
        RuleFor(config => config.InitialTimeout).GreaterThanOrEqualTo(0).When(config => config.IsEnabled).WithMessage("Initial Timeout must be greater than or equal to 0");
        RuleFor(config => config.Interval).GreaterThan(0).When(config => config.IsEnabled).WithMessage("Interval must be greater than 0");
        RuleFor(config => config.UnhealthyInterval).GreaterThan(0).When(config => config.IsEnabled).WithMessage("Unhealthy Interval must be greater than 0");
    }
}