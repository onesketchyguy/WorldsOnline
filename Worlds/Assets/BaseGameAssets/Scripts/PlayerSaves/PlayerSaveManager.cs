using UnityEngine;

namespace Worlds.Saves
{
    public class PlayerSaveManager : MonoBehaviour
    {
        public static void Save(Player.PlayerController player)
        {
            var SaveDat = new SaveData
            {
                userName = player.playerName,
                stats = new int[]
                {
                    player.m_stats.stats.Constitution.value,
                    player.m_stats.stats.Strength.value,
                    player.m_stats.stats.Intelligence.value,
                    player.m_stats.stats.Dexterity.value
                },
                health = player.GetComponent<HealthManager>().GetCurrentHealth(),
                positionX = player.transform.position.x,
                positionY = player.transform.position.y,
                positionZ = player.transform.position.z
            };

            FileManager.Save(SaveDat, SaveDat.userName, FileManager.Directories.saves);
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