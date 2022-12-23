using System.Threading.Tasks.Dataflow;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Hainz.Common.Threading;
using Hainz.Config;
using Hainz.Helpers.Discord;
using Microsoft.Extensions.Logging;

namespace Hainz.Services.Logging;

public sealed class DiscordChannelLoggerService : IAsyncDisposable
{
    private readonly BufferBlock<string> _logQueue;
    private readonly Task _loggerTask;
    private readonly CancellationTokenSource _stopCTS;
    private readonly DiscordSocketClient _client;
    private readonly BotConfig _config;
    private readonly ILogger<DiscordChannelLoggerService> _logger;
    private readonly AsyncManualResetEvent _readyEvent;
    private SocketTextChannel? _logChannel;
    private RestUserMessage? _currentLogMessage;

    public DiscordChannelLoggerService(DiscordSocketClient client,
                                       BotConfig config,
                                       ILogger<DiscordChannelLoggerService> logger) 
    {
        _client = client;
        _config = config;
        _logger = logger;

        _logQueue = new();
        _stopCTS = new CancellationTokenSource();
        _loggerTask = new Task(async () => await LogWriter(_stopCTS.Token));
        _readyEvent = new AsyncManualResetEvent(false);

        if (_client.ConnectionState == ConnectionState.Connected)
            Start();
        
        _client.Ready += () =>
        {
            Start();
            return Task.CompletedTask;
        };

        _client.Disconnected += ex => 
        {
            Stop();
            return Task.CompletedTask;
        };
    }

    public void LogMessage(string logEvent) => _logQueue.Post(logEvent);

    private void Start() 
    {
        _logger.LogInformation("Starting DiscordChannelLoggerService");

        if (_config.Logging.DiscordChannelLogging.IsEnabled) 
        {
            _logChannel = _client.GetChannel(_config.Logging.DiscordChannelLogging.LogChannelId) as SocketTextChannel;
            if (_logChannel == null) 
            {
                _logger.LogWarning("Log channel with id \"{id}\" not found", _config.Logging.DiscordChannelLogging.LogChannelId);
            }
            else 
            {
                _readyEvent.Set();
                _loggerTask.Start();
            }
        }
    }

    private void Stop() 
    {
        _logger.LogInformation("Stopping DiscordChannelLoggerService");
        _readyEvent.Reset();
    }

    private async Task LogWriter(CancellationToken ct) 
    {
        while(!ct.IsCancellationRequested) 
        {
            try 
            {
                await _readyEvent.WaitAsync(ct);

                await foreach (var logMessage in _logQueue.ReceiveAllAsync(ct)) 
                {
                    if (_currentLogMessage != null)
                    {
                       await AppendToLog(_currentLogMessage, logMessage);
                    }
                    else if (_logChannel != null)
                    {
                        var message = DiscordText.WrapInCode(logMessage, "css");
                        _currentLogMessage = await _logChannel.SendMessageAsync(message);
                    }
                }
            }
            catch { }
        }
    }

    private static async Task AppendToLog(RestUserMessage message, string logMessage) 
    {
        await message.ModifyAsync(msg => 
        {
            var content = message.Content;
            int lastLogIndex = content.LastIndexOf('\n');
            msg.Content = content.Insert(lastLogIndex + 1, logMessage + "\n");
        });
    }

    public async ValueTask DisposeAsync()
    {
        _stopCTS.Cancel();
        await _loggerTask;
    }
}