using System.Collections.Generic;
using UnityEngine;

namespace World.Enemies
{
    public static class EnemyBlackBoard
    {
        public static List<Transform> players = new List<Transform>();

        public static Transform[] GetPlayers()
        {
            if (players == null || players.Count < 1) return null;

            for (int i = players.Count - 1; i >= 0; i--)
            {
                // If a player is null we need to remove it,
                if (players[i] == null) players.RemoveAt(i);
            }

            return players.ToArray();
        }
    }
}