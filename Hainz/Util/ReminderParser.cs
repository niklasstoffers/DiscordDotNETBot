using System;
using Hainz.Models;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Discord.WebSocket;

namespace Hainz.Util 
{
    public class ReminderParser 
    {
        private const string MENTION_REGEX = @"<@&?\!?\d+>";

        public Reminder ParseFromMessage(string[] messageParts, SocketUser user, ulong channelId) 
        {
            // basic implementation. will be more sophisticated later
            if (messageParts.Length < 2) return null;
            else if (TimeSpan.TryParse(messageParts[0], out TimeSpan remindIn)) 
            {
                DateTime reminderDate = DateTime.Now;
                TimeSpan add = remindIn.Subtract(reminderDate.TimeOfDay);
                if (remindIn < reminderDate.TimeOfDay)
                    add = add.Add(TimeSpan.FromHours(24));
                reminderDate = reminderDate.Add(add);
                
                string[] mentions = messageParts.Where(p => Regex.IsMatch(p, MENTION_REGEX))
                                               .Where(m => m != user.Mention)
                                               .ToArray();

                var filteredMessage = messageParts.Skip(1)
                                                  .Where(p => !Regex.IsMatch(p, MENTION_REGEX));
                                                  
                string reminderMessage = string.Join(" ", filteredMessage);

                if (string.IsNullOrEmpty(reminderMessage))
                    return null;

                return new Reminder(reminderMessage, reminderDate, user.Id, channelId) 
                {
                    RemindAdditionalUsers = mentions
                };
            }

            return null;
        }        
    }
}