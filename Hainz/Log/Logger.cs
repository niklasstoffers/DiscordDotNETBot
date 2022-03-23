using Hainz.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.Log
{
    public class Logger : IDisposable
    {
        private IOutput _output;

        public Logger(IOutput output)
        {
            _output = output;
        }

        public async Task LogExceptionAsync(Exception ex, LogLevel logLevel = LogLevel.Error)
        {
            string exceptionString = GetExceptionString(ex);
            await LogMessageAsync(exceptionString, logLevel);
        }

        public async Task LogExceptionAsync(AggregateException ex, LogLevel logLevel = LogLevel.Error)
        {
            ex = ex.Flatten();
            string prefix = string.Empty;
            if (ex.InnerExceptions.Count > 1)
            {
                await LogMessageAsync("Aggregate exception thrown. Inner exceptions:", logLevel);
                prefix = "\t";
            }

            foreach (var subException in ex.InnerExceptions)
                await LogMessageAsync($"{prefix}{GetExceptionString(subException)}", logLevel);
        }

        private string GetExceptionString(Exception ex) => $"Exception of type {ex.GetType().Name} thrown with message {ex.Message} inside {ex.StackTrace}";

        public async Task LogMessageAsync(string message, LogLevel logLevel = LogLevel.Debug)
        {
            string log = GetLogString(message, logLevel);

            OutputContextType contextType = logLevel switch
            {
                LogLevel.Critical => OutputContextType.Critical,
                LogLevel.Error => OutputContextType.Error,
                LogLevel.Warning => OutputContextType.Warning,
                LogLevel.Info => OutputContextType.Info,
                LogLevel.Debug => OutputContextType.Debug,
                _ => OutputContextType.Default
            };

            using (var context = await _output.BeginContextAsync(contextType))
            {
                await Log(log);
            }
        }

        private string GetLogString(string message, LogLevel logLevel) => $"{DateTime.Now:G} [{logLevel}]: {message}";

        private async Task Log(string log)
        {
            try
            {
                await _output.AppendAsync(log);
            }
            catch { }
        }

        public void Dispose() => _output.Dispose();
    }
}
