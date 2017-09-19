using System;
using System.Collections.Generic;
using Pingu.Net;
using Pingu.Net.Handler;
using Pingu.Net.Message.Outgoing;
using Pingu.Pingu.Data;

namespace Pingu.Pingu
{
    class Player
    {
        public ClientHandler ClientHandler { get; }
        public string Username { get; }
        public string Hash { get; }
        public PlayerStatus Status { get; }
        public PlayerState State { get; }

        private readonly List<string> _challengedPlayers; 

        public Player(ClientHandler clientHandler, string username, string hash, PlayerStatus status)
        {
            ClientHandler = clientHandler;
            Username = username;
            Hash = hash;
            Status = status;
            State = PlayerState.Lobby;

            _challengedPlayers = new List<string>();
        }

        public void ChallengePlayer(Player player)
        {
            if (!_challengedPlayers.Contains(player.Username))
            {
                _challengedPlayers.Add(player.Username);

                player.ClientHandler.SendMessage(PacketHandler.GetComposer<ComposerRequest>().Compose(this));
            }
            else
            {
                Console.WriteLine("ERROR DOUBLE CHALLENGE ERROR");
            }
        }

        public bool HasChallengedPlayer(Player player)
        {
            return _challengedPlayers.Contains(player.Username);
        }
    }
}
