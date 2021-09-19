using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.Audio
{
    public class MusicPlayer : IDisposable
    {
        public MusicInStream Input { get; set; }
        public event EventHandler Finished;

        public async Task Start()
        {

        }

        public async Task Stop()
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
