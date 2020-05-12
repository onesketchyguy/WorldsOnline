using UnityEngine;
using System.Collections.Generic;

namespace Worlds.Player
{
    public class PlayerStats : MonoBehaviour
    {
        public Stats stats;
        public StatusEffects effects;
    }

    [System.Serializable]
    public class Stats
    {
        /// <summary>
        /// Physical attack damage
        /// </summary>
        public Stat Strength;

        /// <summary>
        /// Non-physical attack damage
        /// </summary>
        public Stat Intelligence;

        /// <summary>
        /// Move speed modifer
        /// </summary>
        public Stat Dexterity;

        /// <summary>
        /// Health modifier
        /// </summary>
        public Stat Constitution;
    }

    [System.Serializable]
    public class Stat
    {
        public const int MIN_VALUE = 1;
        public const int MAX_VALUE = 20;

        [Range(MIN_VALUE, MAX_VALUE)]
        public int value;

        public Dictionary<string, int> modifiers = new Dictionary<string, int>();

        public Stat(int value)
        {
            this.value = value;
        }

        public int GetValue()
        {
            var val = value;

            if (modifiers != null && modifiers.Count > 0)
            {
                foreach (var item in modifiers)
                {
                    val += item.Value;
                }
            }

            return val;
        }

        public void AddModifier(string modKey, int modValue)
            => modifiers.Add(modKey, modValue);

        public void RemoveModifier(string modKey)
        {
            if (modifiers.ContainsKey(modKey))
                modifiers.Remove(modKey);
        }
    }
}