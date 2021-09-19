using System;
using Discord.WebSocket;
using System.Threading.Tasks;
using Hainz.Framework;
using Autofac;
using Hainz.InterfaceCommands;
using System.Threading;
using Hainz.Log;

namespace Hainz
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var application = new Application())
                await application.Run();
        }
    }
}
