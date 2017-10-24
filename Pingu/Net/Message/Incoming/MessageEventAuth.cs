using NLog;
using Pingu.Net.Handler;
using Pingu.Net.Message.Outgoing;
using Pingu.Pingu;
using Pingu.Pingu.Data;
using Pingu.Util;

namespace Pingu.Net.Message.Incoming
{
    internal class MessageEventAuth : IMessageEvent
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void HandleMessage(IncomingMessage message, ClientHandler clientHandler)
        {
            var name = message.Document?.Attribute("name")?.Value;
            var version = message.Document?.Attribute("version")?.Value;
            var sso = message.Document?.Attribute("hash")?.Value;

            if (name == null || version == null || sso == null)
            {
                clientHandler.Disconnect("Invalid MessageEventAuth received.");
                return;
            }

            if (!sso.Equals(Hash.Md5(Program.Config.Salt + name)))
            {
                clientHandler.Disconnect($"SSO mismatch, received '{sso}', expected '{Hash.Md5(Program.Config.Salt + name)}'.");
                return;
            }

            if (PlayerRoom.HasPlayer(name))
            {
                clientHandler.Disconnect("Username already in use.");
                return;
            }

            if (!version.Equals(Constants.Version))
            {
                clientHandler.Disconnect($"Version mismatch, received '{version}', expected '{Constants.Version}'.");
                return;
            }

            Logger.Info($"[{name}] has connected to the server.");

            clientHandler.Username = PlayerRoom.AddPlayer(clientHandler, name, sso, PlayerStatus.Available).Username;
            clientHandler.SendMessage(new[]
            {
                PacketHandler.GetComposer<ComposerConfig>().Compose(),
                PacketHandler.GetComposer<ComposerUserList>().Compose(),
                // PacketHandler.GetComposer<ComposerWarning>().Compose("AeonLucid", "Welkom op de pinguin bomberman private server! Op het moment in Alpha-modus.\n\nDit bericht verdwijnt vanzelf.")
            });
        }
    }
}
