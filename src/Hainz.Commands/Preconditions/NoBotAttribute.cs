using Discord.Commands;
using Discord.WebSocket;
using Hainz.Commands.Helpers;

namespace Hainz.Commands.Preconditions;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public sealed class NoBotAttribute : ParameterPreconditionAttribute
{
    public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, ParameterInfo parameter, object value, IServiceProvider services)
    {
        ulong? userId = value switch
        {
            ulong id => id,
            SocketGuildUser user => user.Id,
            _ => null
        };

        if (userId == null)
        {
            return PreconditionResult.FromError("Invalid precondition application");
        }
        else
        {
            var user = await context.Client.GetUserAsync(userId.Value);

            if (user == null)
                return PreconditionResult.FromError("User not found");
            else
                return PreconditionResultBuilder.SuccessIfNot(user.IsBot, "User must not be a bot");
        }
    }
}