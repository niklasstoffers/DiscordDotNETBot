using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using Hainz.Log;
using Hainz.Models;

namespace Hainz.Util 
{
    public class ReminderService : IDisposable
    {
        private DiscordSocketClient _client;
        private LinkedList<Reminder> _reminders;
        private Task _reminderRunner;
        private TaskCompletionSource _newEarliestReminderTCS;
        private CancellationTokenSource _stopCTS;
        private SemaphoreSlim _lock;
        private Logger _logger;

        public ReminderService(DiscordSocketClient client,
                               Logger logger) 
        {
            _client = client;
            _logger = logger;
            _reminders = new LinkedList<Reminder>();
            _reminderRunner = Task.Run(Run);
            _newEarliestReminderTCS = new TaskCompletionSource();
            _stopCTS = new CancellationTokenSource();
            _lock = new SemaphoreSlim(1, 1);
        }

        private async Task Run() 
        {
            while(!_stopCTS.IsCancellationRequested) 
            {
                var nextReminder = await GetEarliestReminder();
                Task reminderWaiter;
                CancellationTokenSource reminderWaiterCTS = new CancellationTokenSource();

                _stopCTS.Token.Register(() => reminderWaiterCTS.Cancel());

                if (nextReminder != null) 
                {
                    var difference = nextReminder.RemindOn - DateTime.Now;
                    reminderWaiter = Task.Delay(difference, reminderWaiterCTS.Token);
                }
                else 
                {
                    reminderWaiter = Task.Delay(-1, reminderWaiterCTS.Token);
                }
                
                try 
                {
                    await Task.WhenAny(_newEarliestReminderTCS.Task, reminderWaiter);
                    
                    if (_newEarliestReminderTCS.Task.IsCompleted) 
                    {
                        reminderWaiterCTS.Cancel();
                        continue;
                    }
                    else 
                    {
                        await Remind(nextReminder);
                        await RemoveReminder(nextReminder);
                    }
                }
                catch (OperationCanceledException) 
                {
                    break;
                }
                catch (Exception ex) 
                {
                    await _logger.LogMessageAsync("Exception while awaiting reminder", LogLevel.Error);
                    await _logger.LogExceptionAsync(ex);
                }
            }
        }

        private async Task Remind(Reminder reminder) 
        {
            if (_client.ConnectionState == Discord.ConnectionState.Connected) 
            {
                var channel = _client.GetChannel(reminder.RemindInChannelId) as SocketTextChannel;
                var user = _client.GetUser(reminder.RemindUserId);
                
                if (channel != null) 
                {
                    await _logger.LogMessageAsync($"Erinnerung für den {reminder.RemindOn} ausgelöst", LogLevel.Info);
                    string mentions = $"{user.Mention} {string.Join(" ", reminder.RemindAdditionalUsers)}";
                    await channel.SendMessageAsync($"{mentions} hier ist deine Erinnerung: {reminder.Message}");
                }
            }
        }

        private async Task<Reminder> GetEarliestReminder() 
        {
            await _lock.WaitAsync();
            try 
            {
                return _reminders.First?.Value;
            }
            finally 
            {
                _lock.Release();
            }
        }

        public async Task AddReminder(Reminder reminder) 
        {
            if (reminder.RemindOn <= DateTime.Now)
                throw new ArgumentException("Cant add a reminder for a date in the past.");

            await _lock.WaitAsync();
            try 
            {
                if (_reminders.Count == 0)
                    _reminders.AddFirst(reminder);
                else 
                {
                    var currentNode = _reminders.First;
                    LinkedListNode<Reminder> prevNode = null;

                    while(!(currentNode == null || currentNode.Value.RemindOn > reminder.RemindOn)) 
                    {
                        prevNode = currentNode;
                        currentNode = currentNode.Next;
                    }

                    if (prevNode == null) _reminders.AddFirst(reminder);
                    else _reminders.AddAfter(prevNode, reminder);
                }

                _newEarliestReminderTCS.SetResult();
                _newEarliestReminderTCS = new TaskCompletionSource();
            }
            finally 
            {
                _lock.Release();
            }
        }

        public async Task RemoveReminder(Reminder reminder) 
        {
            await _lock.WaitAsync();
            try 
            {
                _reminders.Remove(reminder);
            }
            finally 
            {
                _lock.Release();
            }
        }

        public void Dispose()
        {
            _stopCTS.Cancel();
            _reminderRunner.GetAwaiter().GetResult();
        }
    }
}