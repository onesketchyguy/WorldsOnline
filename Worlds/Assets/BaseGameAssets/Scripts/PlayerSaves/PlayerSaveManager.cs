using UnityEngine;

namespace Worlds.Saves
{
    public class PlayerSaveManager : MonoBehaviour
    {
        public static void Save(Player.Stats stats, string name, float hp, float x = 0, float y = 0, float z = 0)
        {
            var SaveDat = new SaveData
            {
                userName = name,
                stats = new int[]
                {
                    stats.Constitution.value,
                    stats.Strength.value,
                    stats.Intelligence.value,
                    stats.Dexterity.value
                },
                health = hp,
                positionX = x,
                positionY = y,
                positionZ = z
            };

            FileManager.Save(SaveDat, SaveDat.userName, FileManager.Directories.saves);
        }

        public static void Save(Player.PlayerController player)
        {
            Save(player.m_stats.stats, player.playerName, player.GetComponent<HealthManager>().GetCurrentHealth(),
                player.transform.position.x, player.transform.position.y, player.transform.position.z);
        }

        public static SaveData Load(string name)
        {
            return FileManager.Load<SaveData>(name, FileManager.Directories.saves);
        }

        public static SaveData[] LoadAll()
        {
            return FileManager.LoadAllFromDirectory<SaveData>(FileManager.Directories.saves, ".meta");
        }

        [System.Serializable]
        public class SaveData
        {
            public string userName;

            public int[] stats;
            public float health;
            public float positionX, positionY, positionZ;
        }
    }
}