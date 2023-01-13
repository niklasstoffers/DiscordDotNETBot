using FluentValidation;
using Hainz.Data.Configuration.Caching;

namespace Hainz.Data.Validation;

public sealed class RedisConfigurationValidator : AbstractValidator<RedisConfiguration>
{
    public RedisConfigurationValidator()
    {
        RuleFor(config => config.Hostname).NotEmpty().WithMessage("Redis hostname may not be empty");
        RuleFor(config => config.Port).GreaterThan(0).WithMessage("Port must be a valid port");
        RuleFor(config => config.AsyncTimeout).GreaterThan(0).WithMessage("Async timeout must be greater than 0");
        RuleFor(config => config.SyncTimeout).GreaterThan(0).WithMessage("Sync timeout must be greater than 0");
        RuleFor(config => config.ConnectionTimeout).GreaterThan(0).WithMessage("Connection timeout must be greater than 0");
    }
}