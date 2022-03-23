using Discord;
using Hainz.IO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.Log
{
    public class DiscordLogger : IDisposable
    {
        private IOutput _output;

        public DiscordLogger(IOutput output)
        {
            _output = output;
        }

        public async Task Log(Discord.LogMessage arg)
        {
            var contextType = arg.Severity switch
            {
                LogSeverity.Critical => OutputContextType.Critical,
                LogSeverity.Error => OutputContextType.Error,
                LogSeverity.Warning => OutputContextType.Warning,
                LogSeverity.Info => OutputContextType.Info,
                var x when 
                    x == LogSeverity.Debug  ||
                    x == LogSeverity.Verbose => OutputContextType.Debug,
                _ => OutputContextType.Default
            };

            using (var context = await _output.BeginContextAsync(contextType))
            {
                await _output.AppendAsync(arg.ToString());
            }
        }

        public void Dispose() => _output.Dispose();
    }
}
