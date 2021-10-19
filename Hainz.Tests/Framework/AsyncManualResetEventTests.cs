using Hainz.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hainz.Tests.Framework
{
    [TestClass]
    public class AsyncManualResetEventTests
    {
        [TestMethod]
        public async Task AbortTest()
        {
            var mEvent = new AsyncManualResetEvent(false);
            var cts = new CancellationTokenSource();
            var waiter = Task.Run(async () =>
            {
                await mEvent.Wait(cts.Token);
            });
            var canceller = Task.Run(async () => // cancel on different context
            {
                await Task.Delay(2000);
                cts.Cancel();
            });

            await Task.WhenAll(waiter, canceller);
            Assert.IsTrue(true);
        }
    }
}
