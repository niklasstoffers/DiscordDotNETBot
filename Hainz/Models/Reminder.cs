using System;
using Discord.WebSocket;

namespace Hainz.Models 
{
    public class Reminder 
    {
        public string Message { get; set; }
        public DateTime RemindOn { get; set; }
        public ulong RemindUserId { get; set; }
        public ulong RemindInChannelId { get; set; }
        public string[] RemindAdditionalUsers { get; set; }

        public Reminder(string message, 
                        DateTime remindOn,
                        ulong remindUserId,
                        ulong remindInChannelId) 
        {
            Message = message;
            RemindOn = remindOn;
            RemindUserId = remindUserId;
            RemindInChannelId = remindInChannelId;
        }
    }
}