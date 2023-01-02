using FluentValidation;
using Hainz.Data.Configuration.Caching;

namespace Hainz.Data.Validation;

public sealed class RedisConfigurationValidator : AbstractValidator<RedisConfiguration>
{
    public RedisConfigurationValidator()
    {
        RuleFor(config => config.Hostname).NotEmpty();
        RuleFor(config => config.Port).GreaterThan(0);
        RuleFor(config => config.AsyncTimeout).GreaterThan(0);
        RuleFor(config => config.SyncTimeout).GreaterThan(0);
        RuleFor(config => config.ConnectionTimeout).GreaterThan(0);
    }
}