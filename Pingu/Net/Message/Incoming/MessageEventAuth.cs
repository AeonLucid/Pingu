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
        private readonly Logger _logger = LogManager.GetCurrentClassLogger(); // TODO: Remove for better placement

        public void HandleMessage(IncomingMessage message, ClientHandler clientHandler)
        {
            string serverSalt = "liu7gey497t34y#YEYWye7h9y3%@&YQAyutshd"; // TODO: Configurable
            string serverVersion = "1.1.0.speil";

            string name = message.GetDocument().Attribute("name").Value;
            string version = message.GetDocument().Attribute("version").Value;
            string sso = message.GetDocument().Attribute("hash").Value;

            if (sso.Equals(Hash.Md5(serverSalt + name)))
            {
                if (!PlayerRoom.HasPlayer(name))
                {
                    if (version.Equals(serverVersion))
                    {
                        _logger.Info($"'{name}' has connected to the server.");

                        clientHandler.Username = PlayerRoom.AddPlayer(clientHandler, name, sso, PlayerStatus.Available).Username;
                        clientHandler.SendMessages(new[]
                        {
                            PacketHandler.GetComposer<ComposerConfig>().Compose(),
                            PacketHandler.GetComposer<ComposerUserList>().Compose(),
                            //PacketHandler.GetComposer<ComposerWarning>().Compose("AeonLucid", "Welkom op de pinguin bomberman private server! Op het moment in Alpha-modus.\n\nDit bericht verdwijnt vanzelf.")
                        });
                    }
                    else
                    {
                        clientHandler.Disconnect($"Version mismatch, received '{version}', expected '{serverVersion}'.");
                    }
                }
                else
                {
                    clientHandler.Disconnect("Username already in use.");
                }
            }
            else
            {
                clientHandler.Disconnect($"SSO mismatch, received '{sso}', expected '{Hash.Md5(serverSalt + name)}'.");
            }
        }
    }
}
