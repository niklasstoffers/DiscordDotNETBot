using Hainz.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.IO.Outputs
{
    public class FileOutput : OutputBase
    {
        private string _filePath;
        private FileStream _fs;
        private StreamWriter _fileWriter;

        public FileOutput(string filePath)
        {
            _filePath = filePath;
            if (!Path.IsPathFullyQualified(_filePath))
                _filePath = Path.Combine(DirectoryUtil.GetApplicationBaseDirectory(), _filePath);
            TryCreateStreams();
        }

        private void TryCreateStreams()
        {
            try
            {
                _fs = File.OpenWrite(_filePath);
                _fileWriter = new StreamWriter(_fs);
            }
            catch { }
        }

        public override async Task AppendAsync(string message)
        {
            if (_fileWriter != null)
                await _fileWriter.WriteLineAsync(message);
        }

        protected override void Dispose(bool disposing)
        {
            if (!Disposed && disposing)
            {
                _fs?.Dispose();
                _fileWriter?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
