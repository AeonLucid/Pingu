using System;
using System.IO;
using NLog;
using Pingu.Net;
using Pingu.Net.Handler;
using Pingu.Settings;

namespace Pingu
{
    internal class Boot
    {
        public Boot()
        {
            var logNetConfig = new FileInfo(Path.Combine("Settings", "log4net.xml"));
            if (!logNetConfig.Exists)
                throw new Exception("Log4net configuration not found.");

            LogManager.Configuration = NLogConfig.Create();
            LogManager.GetCurrentClassLogger().Debug("Configured NLog..");
        }

        public Server StartServer()
        {
            PacketHandler.Initialize();

            return new Server();
        }
    }
}
