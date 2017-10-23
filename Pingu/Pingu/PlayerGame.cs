using Pingu.Net.Handler;
using Pingu.Net.Message.Outgoing;

namespace Pingu.Pingu
{
    internal class PlayerGame
    {
        public PlayerGame(Player playerOne, Player playerTwo)
        {
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
        }

        /// <summary>
        ///     The <see cref="Player"/> that sent the "MessageEventStartGame".
        /// </summary>
        public Player PlayerOne { get; }

        /// <summary>
        ///     The <see cref="Player"/> that <see cref="PlayerOne"/> clicked on to start the game.
        /// </summary>
        public Player PlayerTwo { get; }

        /// <summary>
        ///     Starts the game.
        /// </summary>
        public void Start()
        {
            PlayerTwo.ClientHandler.SendMessage(PacketHandler.GetComposer<ComposerStartGame>().Compose(PlayerOne.Username));
        }
    }
}
