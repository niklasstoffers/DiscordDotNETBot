using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hainz.IO
{
    public interface IInput
    {
        Task<string> ReceiveNext(CancellationToken ct);
    }
}
