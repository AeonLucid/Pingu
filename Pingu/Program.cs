using System;
using System.Diagnostics;
using System.Threading;
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
