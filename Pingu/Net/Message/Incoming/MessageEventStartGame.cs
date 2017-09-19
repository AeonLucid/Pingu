using Pingu.Net.Handler;
using Pingu.Pingu;
using Pingu.Pingu.Data;

namespace Pingu.Net.Message.Incoming
{
    internal class MessageEventStartGame : IMessageEvent
    {
        // TODO: Error messages

        public void HandleMessage(IncomingMessage message, ClientHandler clientHandler)
        {
            // TODO: Start game
            Player receiver = clientHandler.GetPlayer();

            if (receiver.State == PlayerState.Lobby)
            {
                string challengerUsername = message.GetDocument().Attribute("name").Value;

                if (PlayerRoom.HasPlayer(challengerUsername))
                {
                    Player challenger = PlayerRoom.GetPlayer(challengerUsername);
                    if (challenger.HasChallengedPlayer(receiver))
                    {
                        // TODO: Challenger challenged receiver.
                    }
                }
            }
        }
    }
}
