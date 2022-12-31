using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Hainz.Commands.Preconditions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public sealed class RequireBotAdminPermissionAttribute : PreconditionAttribute
{
    public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
    {
        throw new NotImplementedException();
        // var serverConfig = services.GetRequiredService<ServerConfig>();
        // if (context.User is SocketGuildUser guildUser)
        // {
        //     if (guildUser.Roles.Any(role => role.Name == serverConfig.BotAdminRole))
        //     {
        //         return Task.FromResult(PreconditionResult.FromSuccess());
        //     }
        //     else
        //     {
        //         return Task.FromResult(PreconditionResult.FromError("You do not have the required permissions to invoke this command"));
        //     }
        // }
        // else 
        // {
        //     return Task.FromResult(PreconditionResult.FromError("You need to be in a guild to use this command"));
        // }
    }
}