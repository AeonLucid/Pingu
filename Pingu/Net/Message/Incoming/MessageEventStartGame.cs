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
            var receiver = clientHandler.GetPlayer();
            if (receiver.State == PlayerState.Lobby ||
                receiver.State == PlayerState.GameEnded)
            {
                var challengerUsername = message.Document?.Attribute("name")?.Value;
                if (challengerUsername != null && PlayerRoom.HasPlayer(challengerUsername))
                {
                    var opponent = PlayerRoom.GetPlayer(challengerUsername);
                    if (opponent.HasChallengedPlayer(receiver))
                    {
                        var game = new PlayerGame(receiver, opponent);

                        receiver.StartGame(game);
                        opponent.StartGame(game);

                        game.Start();
                    }
                }
            }
        }
    }
}
