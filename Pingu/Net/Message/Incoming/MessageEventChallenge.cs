using NLog;
using Pingu.Net.Handler;
using Pingu.Pingu;
using Pingu.Pingu.Data;

namespace Pingu.Net.Message.Incoming
{
    internal class MessageEventChallenge : IMessageEvent
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // <challenge name="AeonLucid" hash="xxxxxx" />
        public void HandleMessage(IncomingMessage message, ClientHandler clientHandler)
        {
            var name = message.Document?.Attribute("name")?.Value;
            if (name == null)
            {
                clientHandler.Disconnect("Invalid MessageEventChallenge received.");
                return;
            }

            if (string.Equals(name, clientHandler.Username))
            {
                Logger.Error($"Received self challenge from {name}..?");
                return;
            }

            if (!PlayerRoom.HasPlayer(name))
            {
                return;
            }

            var player = PlayerRoom.GetPlayer(name);
            if (player.State == PlayerState.Lobby)
            {
                clientHandler.GetPlayer().ChallengePlayer(player);
            }
        }
    }
}
