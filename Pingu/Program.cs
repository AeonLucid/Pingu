using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using NLog;
using Pingu.Net;
using Pingu.Net.Handler;
using Pingu.Settings;

namespace Pingu
{
    internal class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly ManualResetEvent KeepRunning = new ManualResetEvent(false);

        public static Config Config { get; private set; }

        private static void Main(string[] args)
        {
            Console.Title = "Pingu - Private Server";
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                KeepRunning.Set();
            };

            LogManager.Configuration = NLogConfig.Create();
            LogManager.GetCurrentClassLogger().Debug("Configured NLog..");

            PacketHandler.Initialize();
            Config = JsonConvert.DeserializeObject<Config>(
                File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Settings", "config.json"))
            );

            using (var server = new Server())
            {
                server.Start();

                KeepRunning.WaitOne();

                server.Stop();
            }

            // In production mode, we do not want to hang with readkey.
            if (!Debugger.IsAttached)
            {
                return;
            }

            Logger.Info("Exiting, press a key to close..");
            Console.ReadKey();
        }
    }
}
