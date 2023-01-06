using FluentValidation;
using Hainz.Data.Configuration.Caching;

namespace Hainz.Data.Validation;

public sealed class CachingConfigurationValidator : AbstractValidator<CachingConfiguration>
{
    public CachingConfigurationValidator()
    {
        RuleFor(config => config.CacheKeyPrefix).NotEmpty();
        RuleFor(config => config.ProviderName).NotEmpty();
        RuleFor(config => config.TimeoutSeconds).GreaterThan(0);

        RuleFor(config => config.Redis)
            .NotNull()
            .SetValidator(new RedisConfigurationValidator());
    }
}