using Discord;
using FluentValidation;
using Hainz.Config.Bot;

namespace Hainz.Config.Validation;

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

        RuleFor(config => config.DefaultActivity)
            .ChildRules(v => 
                v.RuleFor(activity => activity!.Name)
                    .NotEmpty())
            .When(config => config != null);
    }
}