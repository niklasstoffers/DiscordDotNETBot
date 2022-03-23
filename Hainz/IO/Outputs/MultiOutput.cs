using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.IO.Outputs
{
    public class MultiOutput : OutputBase
    {
        private IOutput[] _outputs;

        public MultiOutput(params IOutput[] outputs)
        {
            _outputs = outputs;
        }

        public override async Task AppendAsync(string message)
        {
            foreach (var output in _outputs)
                await output.AppendAsync(message);
        }

        protected override void Dispose(bool disposing)
        {
            foreach (var output in _outputs)
                output.Dispose();
            base.Dispose(disposing);
        }
    }
}
