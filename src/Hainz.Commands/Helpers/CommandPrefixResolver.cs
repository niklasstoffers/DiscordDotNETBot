using Discord;
using Discord.WebSocket;
using Hainz.Commands.Config;
using Hainz.Common.Helpers;
using Hainz.Data.Queries.Guild.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands.Helpers;

public class CommandPrefixResolver
{
    private readonly IMediator _mediator;
    private readonly CommandsConfig _config;
    private readonly ILogger<CommandPrefixResolver> _logger;

    public CommandPrefixResolver(IMediator mediator, CommandsConfig config, ILogger<CommandPrefixResolver> logger)
    {
        _mediator = mediator;
        _config = config;
        _logger = logger;
    }

    public async Task<char> GetPrefix(IChannel channel) => await GetPrefix((channel as SocketGuildChannel)?.Guild.Id);

    public async Task<char> GetPrefix(ulong? guildId = null)
    {
        return await TryWrapper.TryAsync(
            async () => await _mediator.Send(new GetCommandPrefixQuery(guildId ?? 0)),
            _config.FallbackPrefix,
            ex => _logger.LogError(ex, "Error while trying to command prefix. Using fallback prefix instead.")
        );
    }
}