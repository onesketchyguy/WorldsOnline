using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Worlds.UI
{
    using Player;

    public class StatsPanel : MonoBehaviour
    {
        public Text text;

        private PlayerStats playerStats;

        private void SetStats()
        {
            var list = FindObjectsOfType<PlayerController>();
            foreach (var item in list)
            {
                if (item.isLocalPlayer)
                {
                    playerStats = item.m_stats;
                }
            }
        }

        private void Update()
        {
            if (playerStats == null)
            {
                SetStats();
            }

            text.text = $"Constitution: {playerStats.stats.Constitution.value}\n" +
                        $"Dexterity: {playerStats.stats.Dexterity.value}\n" +
                        $"Strength: {playerStats.stats.Strength.value}\n" +
                        $"Intelligence: {playerStats.stats.Intelligence.value}\n";

            if (playerStats.effects != null && playerStats.effects.effects != null && playerStats.effects.effects.Count > 0)
                foreach (var item in playerStats.effects.effects)
                {
                    text.text += $"\n{item.status}";
                }
            else text.text += "\nYou feel fine.";
        }
    }
}