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

        private PlayerStats stats;

        private void SetStats()
        {
            var list = FindObjectsOfType<PlayerController>();
            foreach (var item in list)
            {
                if (item.isLocalPlayer)
                {
                    stats = item.m_stats;
                }
            }
        }

        private void Update()
        {
            if (stats == null)
            {
                SetStats();
            }

            text.text = $"Constitution: {stats.stats.Constitution.value}\n" +
                        $"Dexterity: {stats.stats.Dexterity.value}\n" +
                        $"Strength: {stats.stats.Strength.value}\n" +
                        $"Intelligence: {stats.stats.Intelligence.value}\n";

            if (stats.effects != null && stats.effects.effects != null && stats.effects.effects.Count > 0)
                foreach (var item in stats.effects.effects)
                {
                    text.text += $"\n{item.status}";
                }
            else text.text += "\nYou feel fine.";
        }
    }
}