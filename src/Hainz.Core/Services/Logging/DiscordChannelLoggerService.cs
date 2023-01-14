using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Hainz.Data.Queries.Channels.LogChannels.GetLogChannels;
using Hainz.Hosting;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Core.Services.Logging;

[RequireGatewayConnection]
public sealed class DiscordChannelLoggerService : GatewayServiceBase, IDisposable
{
    private readonly BufferBlock<string> _logQueue;
    private readonly DiscordSocketClient _client;
    private readonly IMediator _mediator;
    private readonly ILogger<DiscordChannelLoggerService> _logger;
    private readonly SemaphoreSlim _channelLoadLock;
    private Task? _loggerTask;
    private bool _isEnabled = true;
    private CancellationTokenSource? _stopCTS;
    private ConcurrentBag<SocketTextChannel>? _logChannels;
    private ConcurrentDictionary<ulong, RestUserMessage?>? _currentLogMessages;

    public DiscordChannelLoggerService(DiscordSocketClient client,
                                       IMediator mediator,
                                       ILogger<DiscordChannelLoggerService> logger) 
    {
        _client = client;
        _mediator = mediator;
        _logger = logger;

        _channelLoadLock = new SemaphoreSlim(1, 1);
        _logQueue = new();
    }

    public void LogMessage(string logEvent)
    {
        if (_isEnabled)
            _logQueue.Post(logEvent);
    }

    public override async Task StartAsync() 
    {
        _logger.LogInformation("Starting DiscordChannelLoggerService");
        
        _currentLogMessages = new();
        await ReloadLogChannels();

        if (!_logChannels!.IsEmpty)
        {
            _logger.LogInformation("Starting channel logger task");
            _isEnabled = true;
            _stopCTS = new();
            _loggerTask = Task.Run(async () => await LogWriterAsync(_stopCTS.Token));
        }
        else
        {
            _isEnabled = false;
            _logger.LogInformation("No log channels found. Skipping creation of logger task.");
        }
    }

    public override async Task StopAsync() 
    {
        _logger.LogInformation("Stopping DiscordChannelLoggerService");
        _stopCTS?.Cancel();
        await (_loggerTask ?? Task.CompletedTask);
    }

    public async Task Reload() => await ReloadLogChannels();

    private async Task ReloadLogChannels()
    {
        try
        {
            _logger.LogInformation("Reloading log channels");
            
            await _channelLoadLock.WaitAsync();
            _logChannels = new();

            var logChannelDTOs = await _mediator.Send(new GetLogChannelsQuery());
            await Parallel.ForEachAsync(logChannelDTOs, async (channel, cancellationToken) =>
            {
                if (await _client.GetChannelAsync(channel.ChannelId) is SocketTextChannel logChannel)
                {
                    _logChannels.Add(logChannel);
                    _logger.LogTrace("Using channel with id {id} for logging", channel.ChannelId);
                }
                else
                    _logger.LogInformation("Skipping channel with id {id} because it could not be converted to a text channel", channel.ChannelId);
            });

            _logger.LogInformation("Number of log channels: {num}", _logChannels.Count);
        }
        finally
        {
            _channelLoadLock.Release();
        }
    }

    private async Task LogWriterAsync(CancellationToken ct) 
    {
        while(!ct.IsCancellationRequested) 
        {
            try 
            {
                await foreach (var logMessage in _logQueue.ReceiveAllAsync(ct)) 
                {
                    try
                    {
                        await _channelLoadLock.WaitAsync(ct);
                        foreach (var logChannel in _logChannels!)
                            await AppendToLogAsync(logMessage, logChannel);
                    }
                    finally
                    {
                        _channelLoadLock.Release();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in LogWriter task");
            }
        }
    }

    private async Task AppendToLogAsync(string logMessage, SocketTextChannel logChannel) 
    {
        var currentMessage = _currentLogMessages!.GetValueOrDefault(logChannel.Id);
        if (currentMessage == null || 
            logMessage.Length + currentMessage.Content.Length + 1 > DiscordConfig.MaxMessageSize)
        {
            await WriteAsNewMessageAsync(logMessage, logChannel);
        }
        else
        {
            await currentMessage.ModifyAsync(msg => 
            {
                var content = currentMessage.Content;
                int lastLogIndex = content.LastIndexOf('\n');
                msg.Content = content.Insert(lastLogIndex + 1, logMessage + "\n");
            });
        }
    }

    private async Task WriteAsNewMessageAsync(string logMessage, SocketTextChannel logChannel)
    {
        logMessage = Format.Code(logMessage, "css");
        var currentMessage = _currentLogMessages!.GetValueOrDefault(logChannel.Id);

        if (logMessage.Length > DiscordConfig.MaxMessageSize)
        {
            // Note: Although the message could be split and send in separate parts we decided not to do that.
            _logger.LogWarning("Not logging log message to Discord channel because content length of {contentSize} exceeds maximum length {maximumLength}", logMessage.Length, DiscordConfig.MaxMessageSize);
        }
        else
        {
            MessageReference? oldLogMessageReference = null;
            if (currentMessage != null)
                oldLogMessageReference = new MessageReference(currentMessage.Id);

            _currentLogMessages![logChannel.Id] = await logChannel.SendMessageAsync(logMessage, messageReference: oldLogMessageReference);   
        }
    }

    public void Dispose()
    {
        _stopCTS?.Dispose();
    }
}