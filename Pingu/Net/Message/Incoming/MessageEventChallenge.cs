using System;
using Pingu.Net.Handler;
using Pingu.Pingu;
using Pingu.Pingu.Data;

namespace Pingu.Net.Message.Incoming
{
    internal class MessageEventChallenge : IMessageEvent
    {
        // <challenge name="AeonLucid" hash="xxxxxx" />
        public void HandleMessage(IncomingMessage message, ClientHandler clientHandler)
        {
            string name = message.GetDocument().Attribute("name").Value;

            if (!string.Equals(name, clientHandler.Username, StringComparison.CurrentCultureIgnoreCase))
            {
                if (PlayerRoom.HasPlayer(name))
                {
                    Player player = PlayerRoom.GetPlayer(name);
                    if (player.State == PlayerState.Lobby)
                    {
                        Player sender = clientHandler.GetPlayer();
                        sender.ChallengePlayer(player);
                    }
                }
            }
            else
            {
                // TODO: Send error
                Console.WriteLine("ERROR ERROR ERROR, Username mismatch");
            }
        }
    }
}
