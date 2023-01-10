using FluentValidation;
using Hainz.Data.Configuration.Caching;

namespace Hainz.Data.Validation;

public sealed class CachingConfigurationValidator : AbstractValidator<CachingConfiguration>
{
    public CachingConfigurationValidator()
    {
        RuleFor(config => config.CacheKeyPrefix).NotEmpty().WithMessage("Cache key prefix may not be empty");
        RuleFor(config => config.ProviderName).NotEmpty().WithMessage("Provider name may not be empty");
        RuleFor(config => config.TimeoutSeconds).GreaterThan(0).WithMessage("Timeout seconds must be greater than 0");

        RuleFor(config => config.Redis)
            .NotNull()
            .SetValidator(new RedisConfigurationValidator())
            .WithMessage("Redis configuration may not be null");
    }
}