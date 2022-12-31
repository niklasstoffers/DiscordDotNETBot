using Discord;
using FluentValidation;
using Hainz.Core.Config;

namespace Hainz.Core.Validation.Configuration;

public sealed class BotConfigValidator : AbstractValidator<BotConfig>
{
    public BotConfigValidator()
    {
        RuleFor(config => config.Token)
            .Custom((token, context) => 
            {
                try
                {
                    TokenUtils.ValidateToken(TokenType.Bot, token);
                }
                catch 
                {
                    context.AddFailure(context.PropertyName, "Invalid bot token");
                }
            });
    }
}