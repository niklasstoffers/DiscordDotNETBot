using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using Hainz.Commands.TypeReaders;
using Hainz.Common.Helpers;
using Hainz.Data.Queries.Guild.Commands;
using Hainz.Events.Notifications.Messages;
using Hainz.Hosting;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands;

public sealed class CommandHandler : GatewayServiceBase, INotificationHandler<MessageReceived>
{
    private const char DEFAULT_COMMAND_PREFIX = '!';

    private readonly DiscordSocketClient _client;
    private readonly CommandService _commandService;
    private readonly IMediator _mediator;
    private readonly IEnumerable<TypeReaderBase> _typeReaders;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CommandHandler> _logger;

    public CommandHandler(DiscordSocketClient client, 
                          CommandService commands,
                          IMediator mediator,
                          IEnumerable<TypeReaderBase> typeReaders,
                          IServiceProvider serviceProvider,
                          ILogger<CommandHandler> logger)
    {
        _client = client;
        _commandService = commands;
        _mediator = mediator;
        _typeReaders = typeReaders;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public override async Task SetupAsync()
    {
        _logger.LogTrace("Adding type readers");
        AddTypeReaders();

        try 
        {
            _logger.LogTrace("Adding command modules");
            await _commandService.AddModulesAsync(assembly: Assembly.GetExecutingAssembly(), 
                                                  services: _serviceProvider);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Error while trying to add command modules");
        }
    }

    private void AddTypeReaders()
    {
        foreach (var typeReader in _typeReaders)
        {
            _logger.LogTrace("Adding type reader {name} for type {type}", typeReader.GetType().FullName, typeReader.ForType.FullName);
            _commandService.AddTypeReader(typeReader.ForType, typeReader);
        }
    }

    public async Task Handle(MessageReceived notification, CancellationToken cancellationToken)
    {
        var message = notification.Message;
        _logger.LogTrace("Received message \"{message}\" from \"{user}\"", message.Content, message.Author.Username);

        if (message is not SocketUserMessage userMessage)
            return;

        ulong guildId = 0;
        int argPos = 0;

        if (userMessage.Channel is SocketGuildChannel guildChannel)
            guildId = guildChannel.Guild.Id;

        char commandPrefix = await TryWrapper.TryAsync(
            async () => await _mediator.Send(new GetCommandPrefixQuery(guildId), cancellationToken),
            DEFAULT_COMMAND_PREFIX,
            ex => _logger.LogError(ex, "Error while trying to retrieve command prefix from database. Using default prefix instead")
        );

        if (!(userMessage.HasCharPrefix(commandPrefix, ref argPos) || 
            userMessage.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
            userMessage.Author.IsBot)
            return;

        _logger.LogInformation("Received command \"{command}\" from \"{user}\"", message.Content, message.Author.Username);
        var context = new SocketCommandContext(_client, userMessage);

        try 
        {
            _logger.LogTrace("Executing command");
            await _commandService.ExecuteAsync(
                context: context, 
                argPos: argPos,
                services: _serviceProvider);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Exception during command execution");
        }
    }
}