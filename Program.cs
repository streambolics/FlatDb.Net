using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FlatDatabase
{
    public class Program
    {
        private static CancellationTokenSource _Cancellation = new CancellationTokenSource();
        private static Secret _Secret = new Secret();
        private static Arguments _Arguments;

        public static bool CheckAdminSecret(string aPassword) => _Secret.Check(aPassword);


        /// <summary>
        ///     Shuts down the server.
        /// </summary>

        public static void Shutdown() => _Cancellation.CancelAfter(5000);

        public static void Main(string[] args)
        {
            _Arguments = new Arguments(args);
            _Arguments.Parse("--autostop", v => _Cancellation.CancelAfter(v));
            _Arguments.Parse("--admintoken", s => _Secret.SetPassword(s));
            BuildWebHost(_Arguments.OwnerArguments).RunAsync (_Cancellation.Token).Wait();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
