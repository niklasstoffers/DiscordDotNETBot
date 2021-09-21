using System;
using System.Threading.Tasks;
using Discord.Commands;
using Hainz.Log;
using Hainz.Util;
using System.Linq;

namespace Hainz.Commands.Modules 
{
    public class ReminderModule : ModuleBase<SocketCommandContext>
    {
        private ReminderService _reminderService;
        private ReminderParser _reminderParser;
        private Logger _logger;

        public ReminderModule(ReminderService reminderService,
                              ReminderParser reminderParser,
                              Logger logger) 
        {
            _reminderService = reminderService;
            _reminderParser = reminderParser;
            _logger = logger;
        }

        [Command("reminder", RunMode = RunMode.Async)]
        public async Task ReminderAsync(params string[] reminderMsgParts) 
        {
            ulong channelId = Context.Channel.Id;
            var reminder = _reminderParser.ParseFromMessage(reminderMsgParts, Context.Message.Author, channelId);

            if (reminder == null) 
            {
                await Context.Channel.SendMessageAsync("Fehler beim Hinzufügen der Erinnerung");
            }
            else if (reminder.RemindOn < DateTime.Now) 
            {
                await Context.Channel.SendMessageAsync("Ich kann dich nicht für etwas erinnern das in der Vergangenheit liegt.");
            }
            else 
            {
                await _logger.LogMessageAsync($"Erinnerung für den {reminder.RemindOn} hinzugefügt", LogLevel.Info);
                await _reminderService.AddReminder(reminder);
                await Context.Channel.SendMessageAsync("Erinnerung gesetzt.");
            }
        }
    }
}