using System;
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
