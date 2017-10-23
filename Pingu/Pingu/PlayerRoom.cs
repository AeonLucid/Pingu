using System;
using System.Collections.Generic;
using System.Linq;
using Pingu.Net;
using Pingu.Net.Handler;
using Pingu.Net.Message;
using Pingu.Net.Message.Outgoing;
using Pingu.Pingu.Data;

namespace Pingu.Pingu
{
    internal static class PlayerRoom
    {
        public static readonly Dictionary<string, Player> Players = new Dictionary<string, Player>();

        public static Player AddPlayer(ClientHandler clientHandler, string username, string hash, PlayerStatus status)
        {
            var player = new Player(clientHandler, username, hash, status);

            Players.Add(username.ToLower(), player);

            SendMessageToAll(PacketHandler.GetComposer<ComposerPlayerNew>().Compose(player), username);

            return player;
        }

        public static Player GetPlayer(string username)
        {
            return Players[username.ToLower()];
        }

        public static void RemovePlayer(string username)
        {
            var player = GetPlayer(username);

            SendMessageToAll(PacketHandler.GetComposer<ComposerPlayerLeft>().Compose(player), username);
            Players.Remove(username.ToLower());
        }

        public static bool HasPlayer(string username)
        {
            return Players.Values.Any(player => username.ToLower().Equals(player.Username.ToLower()));
        }

        public static void SendChatMessage(string username, string message)
        {
            var outgoingMessage = PacketHandler.GetComposer<ComposerMessageAll>().Compose(username, message);

            foreach (var player in Players.Values.Where(p => !string.Equals(p.Username, username)).Where(player => player.State == PlayerState.Lobby))
            {
                player.ClientHandler.SendMessage(outgoingMessage);
            }
        }

        private static void SendMessageToAll(OutgoingMessage outgoingMessage)
        {
            foreach (var player in Players.Values)
            {
                player.ClientHandler.SendMessage(outgoingMessage);
            }
        }

        private static void SendMessageToAll(OutgoingMessage outgoingMessage, string ignoreUsername)
        {
            foreach (var player in Players.Values.Where(p => !string.Equals(p.Username, ignoreUsername)))
            {
                player.ClientHandler.SendMessage(outgoingMessage);
            }
        }
    }
}
