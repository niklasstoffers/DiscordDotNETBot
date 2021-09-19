using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.IO
{
    public interface IOutput : IDisposable
    {
        Task AppendAsync(string message);
        Task<OutputContext> BeginContextAsync(OutputContextType contextType);
    }
}
