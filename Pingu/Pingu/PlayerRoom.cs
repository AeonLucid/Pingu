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
            Player player = new Player(clientHandler, username, hash, status);
            Players.Add(username.ToLower(), player);

            SendMessageToAllExcept(PacketHandler.GetComposer<ComposerPlayerNew>().Compose(player), username);

            return player;
        }

        public static Player GetPlayer(string username)
        {
            return Players[username.ToLower()];
        }

        public static void RemovePlayer(string username)
        {
            Player player = GetPlayer(username);

            SendMessageToAllExcept(PacketHandler.GetComposer<ComposerPlayerLeft>().Compose(player), username);
            Players.Remove(username.ToLower());
        }

        public static bool HasPlayer(string username)
        {
            return Players.Values.Any(player => username.ToLower().Equals(player.Username.ToLower()));
        }

        public static void SendChatMessage(string username, string message)
        {
            OutgoingMessage outgoingMessage = PacketHandler.GetComposer<ComposerMessageAll>().Compose(username, message);

            foreach (Player player in Players.Values.Where(p => !string.Equals(p.Username, username, StringComparison.CurrentCultureIgnoreCase)).Where(player => player.State == PlayerState.Lobby))
            {
                player.ClientHandler.SendMessage(outgoingMessage);
            }
        }

        private static void SendMessageToAll(OutgoingMessage outgoingMessage)
        {
            foreach (Player player in Players.Values)
            {
                player.ClientHandler.SendMessage(outgoingMessage);
            }
        }

        private static void SendMessageToAllExcept(OutgoingMessage outgoingMessage, string ignoreUsername)
        {
            foreach (Player player in Players.Values.Where(p => !string.Equals(p.Username, ignoreUsername, StringComparison.CurrentCultureIgnoreCase)))
            {
                player.ClientHandler.SendMessage(outgoingMessage);
            }
        }
    }
}
