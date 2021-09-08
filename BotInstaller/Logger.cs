using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BotInstaller
{
    public class Logger : IDisposable
    {
        private string _logFolder;
        private string _fileName;
        private FileStream _fs;
        private StreamWriter _writer;

        public Logger(string logFolder)
        {
            _logFolder = logFolder;
            _fileName = $"install{DateTime.Now.GetHashCode()}.log";
            Init();
        }

        private void Init()
        {
            try
            {
                Directory.CreateDirectory(_logFolder);
                string path = Path.Combine(_logFolder, _fileName);
                _fs = File.Create(path);
                _writer = new StreamWriter(_fs);
            }
            catch (Exception ex) 
            { }
        }

        public void LogMessage(string message) =>
            Write($"{DateTime.Now} (message): {message}");

        public void LogException(Exception ex) =>
            Write($"{DateTime.Now} (exception): {ex.Message} in {ex.StackTrace}");

        private void Write(string message)
        {
            try
            {
                _writer.WriteLine(message);
            }
            catch { }
        }

        public void Dispose()
        {
            _writer?.Dispose();
            _fs?.Dispose();
        }
    }
}
