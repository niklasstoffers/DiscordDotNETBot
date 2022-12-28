using Discord.Commands;
using Discord.WebSocket;
using Hainz.Commands.Helpers;

namespace Hainz.Commands.Preconditions;

[AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
public sealed class NotSelfInvokableAttribute : ParameterPreconditionAttribute
{
    public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, ParameterInfo parameter, object value, IServiceProvider services)
    {
        ulong? userId = value switch 
        {
            ulong id => id,
            SocketGuildUser user => user.Id,
            _ => null
        };

        if (userId == null)
            return Task.FromResult(PreconditionResult.FromError("Invalid precondition application"));

        return Task.FromResult(PreconditionResultBuilder.SuccessIfNot(userId == context.User.Id, "Cannot invoke command on self"));
    }
}