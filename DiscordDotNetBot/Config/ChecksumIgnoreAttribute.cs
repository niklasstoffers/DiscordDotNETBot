using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordDotNetBot.Config
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    sealed class ChecksumIgnoreAttribute : Attribute
    {
        public ChecksumIgnoreAttribute()
        {
        }
    }
}
