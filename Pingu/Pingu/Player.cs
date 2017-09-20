using System;
using System.Collections.Generic;
using Pingu.Net;
using Pingu.Net.Handler;
using Pingu.Net.Message.Outgoing;
using Pingu.Pingu.Data;

namespace Pingu.Pingu
{
    internal class Player
    {
        public Player(ClientHandler clientHandler, string username, string hash, PlayerStatus status)
        {
            ClientHandler = clientHandler;
            Username = username;
            Hash = hash;
            Status = status;
            State = PlayerState.Lobby;
            ChallengedPlayers = new List<string>();
        }

        public ClientHandler ClientHandler { get; }

        public string Username { get; }

        public string Hash { get; }

        public PlayerStatus Status { get; }

        public PlayerState State { get; }

        private List<string> ChallengedPlayers { get; }

        public void ChallengePlayer(Player player)
        {
            if (!ChallengedPlayers.Contains(player.Username))
            {
                ChallengedPlayers.Add(player.Username);

                player.ClientHandler.SendMessage(PacketHandler.GetComposer<ComposerRequest>().Compose(this));
            }
            else
            {
                Console.WriteLine("ERROR DOUBLE CHALLENGE ERROR");
            }
        }

        public bool HasChallengedPlayer(Player player)
        {
            return ChallengedPlayers.Contains(player.Username);
        }
    }
}
