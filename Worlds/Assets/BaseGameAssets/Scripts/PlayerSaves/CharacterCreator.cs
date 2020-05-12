using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Worlds.Player
{
    using Saves;

    public class CharacterCreator : MonoBehaviour
    {
        public UnityEngine.Events.UnityEvent OnContinueSuccess;
        public UnityEngine.Events.UnityEvent OnExit;

        private string userName;

        private Stats stats = new Stats();

        private int points;

        public UnityEngine.UI.Text pointsText;

        public UnityEngine.UI.Text StrengthValue, DexterityValue, ConstitutionValue, IntelligenceValue;

        public void SetUserName(string name)
        {
            userName = name;
        }

        public void ContinueToCreationPage()
        {
            if (string.IsNullOrWhiteSpace(userName)) return;

            OnContinueSuccess.Invoke();

            var pb = Stat.MAX_VALUE * 2.75f;

            var str = Random.Range(Stat.MIN_VALUE, Stat.MAX_VALUE);
            pb -= str;
            var con = Random.Range(Stat.MIN_VALUE, Stat.MAX_VALUE);
            pb -= con;

            var @int = 0;

            if (pb >= Stat.MAX_VALUE)
            {
                @int = Random.Range(Stat.MIN_VALUE, Stat.MAX_VALUE);
            }
            else @int = (int)Random.Range(Stat.MIN_VALUE, pb);

            pb -= @int;

            var dex = (int)Random.Range(Stat.MIN_VALUE, pb);

            stats = new Stats()
            {
                Strength = new Stat(str),
                Constitution = new Stat(con),
                Intelligence = new Stat(@int),
                Dexterity = new Stat(dex)
            };

            UpdateStats();
        }

        public void SaveAndExit()
        {
            PlayerSaveManager.Save(stats, userName, 100);

            var networkdManager = FindObjectOfType<WorldNetworkManager>();
            networkdManager.PlayerName = networkdManager.playerData.name = userName;
            networkdManager.playerData = new WorldNetworkManager.CreatePlayerMessage
            {
                Strength = stats.Strength.value,
                Intelligence = stats.Intelligence.value,
                Dexterity = stats.Dexterity.value,
                Constitution = stats.Constitution.value
            };

            OnExit.Invoke();
        }

        public void IncreaseStat(string stat)
        {
            var val = 1;

            if (points - val < 0) return;

            if (stat.ToUpper() == "Dexterity".ToUpper())
            {
                if (stats.Dexterity.value < Stat.MAX_VALUE)
                    stats.Dexterity.value += val;
            }

            if (stat.ToUpper() == "Constitution".ToUpper())
            {
                if (stats.Constitution.value < Stat.MAX_VALUE)
                    stats.Constitution.value += val;
            }

            if (stat.ToUpper() == "Intelligence".ToUpper())
            {
                if (stats.Intelligence.value < Stat.MAX_VALUE)
                    stats.Intelligence.value += val;
            }

            if (stat.ToUpper() == "Strength".ToUpper())
            {
                if (stats.Strength.value < Stat.MAX_VALUE)
                    stats.Strength.value += val;
            }

            points += -val;

            UpdateStats();
        }

        public void DecreaseStat(string stat)
        {
            var val = -1;

            if (stat.ToUpper() == "Dexterity".ToUpper())
            {
                if (stats.Dexterity.value > Stat.MIN_VALUE)
                    stats.Dexterity.value += val;
            }

            if (stat.ToUpper() == "Constitution".ToUpper())
            {
                if (stats.Constitution.value > Stat.MIN_VALUE)
                    stats.Constitution.value += val;
            }

            if (stat.ToUpper() == "Intelligence".ToUpper())
            {
                if (stats.Intelligence.value > Stat.MIN_VALUE)
                    stats.Intelligence.value += val;
            }

            if (stat.ToUpper() == "Strength".ToUpper())
            {
                if (stats.Strength.value > Stat.MIN_VALUE)
                    stats.Strength.value += val;
            }

            points += -val;

            UpdateStats();
        }

        private void UpdateStats()
        {
            pointsText.text = $"Points remaining: {points}";
            StrengthValue.text = $"{stats.Strength.value}";
            DexterityValue.text = $"{stats.Dexterity.value}";
            ConstitutionValue.text = $"{stats.Constitution.value}";
            IntelligenceValue.text = $"{stats.Intelligence.value}";
        }
    }
}