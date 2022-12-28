using System.Threading.Tasks.Dataflow;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Hainz.Core.Config.Server;
using Hainz.Core.Config.Server.Channels;
using Hainz.Hosting;
using Microsoft.Extensions.Logging;

namespace Hainz.Logging.Services;

[RequireGatewayConnection]
public sealed class DiscordChannelLoggerService : GatewayServiceBase
{
    private readonly BufferBlock<string> _logQueue;
    private readonly DiscordSocketClient _client;
    private readonly LogChannelConfig? _logChannelConfig;
    private readonly ILogger<DiscordChannelLoggerService> _logger;
    private Task? _loggerTask;
    private CancellationTokenSource _stopCTS;
    private SocketTextChannel? _logChannel;
    private RestUserMessage? _currentLogMessage;

    public DiscordChannelLoggerService(DiscordSocketClient client,
                                       ServerConfig serverConfig,
                                       ILogger<DiscordChannelLoggerService> logger) 
    {
        _client = client;
        _logChannelConfig = serverConfig.Channels?.LogChannel;
        _logger = logger;

        _logQueue = new();
        _stopCTS = new();
    }

    public void LogMessage(string logEvent) => _logQueue.Post(logEvent);

    public override async Task StartAsync() 
    {
        _logger.LogInformation("Starting DiscordChannelLoggerService");

        if (_logChannelConfig?.IsEnabled ?? false) 
        {
            var logChannelId = _logChannelConfig.ChannelId;
            _logChannel = await _client.GetChannelAsync(logChannelId) as SocketTextChannel;
            if (_logChannel == null) 
            {
                _logger.LogWarning("Log channel with id \"{id}\" not found", logChannelId);
            }
            else 
            {
                _logger.LogInformation("Starting channel logger task");
                _stopCTS = new();
                _loggerTask = Task.Run(async () => await LogWriterAsync(_stopCTS.Token));
            }
        }
        else
        {
            _logger.LogInformation("Discord channel logging is disabled");
        }
    }

    public override async Task StopAsync() 
    {
        _logger.LogInformation("Stopping DiscordChannelLoggerService");
        _stopCTS.Cancel();
        await (_loggerTask ?? Task.CompletedTask);
    }

    private async Task LogWriterAsync(CancellationToken ct) 
    {
        while(!ct.IsCancellationRequested) 
        {
            try 
            {
                await foreach (var logMessage in _logQueue.ReceiveAllAsync(ct)) 
                {
                    await AppendToLogAsync(logMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in LogWriter task");
            }
        }
    }

    private async Task AppendToLogAsync(string logMessage) 
    {
        if (_currentLogMessage == null || 
            logMessage.Length + _currentLogMessage.Content.Length + 1 > DiscordConfig.MaxMessageSize)
        {
            await WriteAsNewMessageAsync(logMessage);
        }
        else
        {
            await _currentLogMessage.ModifyAsync(msg => 
            {
                var content = _currentLogMessage.Content;
                int lastLogIndex = content.LastIndexOf('\n');
                msg.Content = content.Insert(lastLogIndex + 1, logMessage + "\n");
            });
        }
    }

    private async Task WriteAsNewMessageAsync(string logMessage)
    {
        logMessage = Format.Code(logMessage, "css");

        if (logMessage.Length > DiscordConfig.MaxMessageSize)
        {
            // Note: Although the message could be split and send in separate parts we decided not to do that.
            _logger.LogWarning("Not logging log message to Discord channel because content length of {contentSize} exceeds maximum length {maximumLength}", logMessage.Length, DiscordConfig.MaxMessageSize);
        }
        else
        {
            MessageReference? oldLogMessageReference = null;
            if (_currentLogMessage != null)
                oldLogMessageReference = new MessageReference(_currentLogMessage.Id);

            _currentLogMessage = await _logChannel!.SendMessageAsync(logMessage, messageReference: oldLogMessageReference);   
        }
    }
}