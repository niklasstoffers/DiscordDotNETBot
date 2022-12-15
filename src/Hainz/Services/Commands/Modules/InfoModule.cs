using Discord.Commands;
using Microsoft.Extensions.Logging;

namespace Hainz.Services.Commands.Modules;

public class InfoModule : ModuleBase<SocketCommandContext>
{
    private ILogger<InfoModule> _logger;

    public InfoModule(ILogger<InfoModule> logger) 
    {
        _logger = logger;
    }

    [Command("ping")]
    public async Task Ping() 
    {
        await Context.Channel.SendMessageAsync("pong");
    }
}