using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Pingu
{
    internal static class Constants
    {
        public const string Version = "1.1.0.speil";

        public static readonly Regex PacketFix = new Regex("<(?<number>\\d+) ", RegexOptions.Compiled);

        public static readonly Dictionary<string, string> PacketFixMap = new Dictionary<string, string>
        {
            { "10", "gameBombExplode" },
            { "11", "gameBombStop" },
            { "12", "gameSendKey" },
            { "14", "gameSendPing" },
            { "16", "gameSetSeed" },
            { "17", "gameBombSet" },
            { "18", "gamePowerupPickup" },
            { "19", "gameBombKick" },
        };
    }
}
