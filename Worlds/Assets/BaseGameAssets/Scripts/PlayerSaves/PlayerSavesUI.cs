using UnityEngine;
using UnityEngine.UI;

namespace Worlds.UI
{
    using Saves;

    public class PlayerSavesUI : MonoBehaviour
    {
        public UnityEngine.Events.UnityEvent OnLoadEvent;
        public GameObject buttonPrefab;

        private void DestroyChildren(Transform parent)
        {
            foreach (var item in parent.GetComponentsInChildren<Transform>())
            {
                if (item == parent) continue;
                Destroy(item.gameObject);
            }
        }

        public void CreateButtons(Transform parent)
        {
            DestroyChildren(parent);

            var saveData = PlayerSaveManager.LoadAll();
            if (saveData == null || saveData.Length < 1) return;
            foreach (var save in saveData)
            {
                if (save == null || string.IsNullOrEmpty(save.userName)) continue;

                CreateButton(save, parent);
            }
        }

        private void CreateButton(PlayerSaveManager.SaveData saveData, Transform parent)
        {
            var go = Instantiate(buttonPrefab, parent);
            var button = go.GetComponent<Button>();
            var image = go.GetComponent<Image>();
            var text = go.GetComponentInChildren<Text>();
            text.text = $"{saveData.userName}";
            text.alignment = TextAnchor.MiddleCenter;

            button.onClick.AddListener(() => LoadCharacter(saveData)); // Load;
            button.targetGraphic = image;
        }

        public void LoadCharacter(PlayerSaveManager.SaveData saveData)
        {
            OnLoadEvent.Invoke(); // Debug
            // TODO: Load in the character data
            var networkdManager = FindObjectOfType<WorldNetworkManager>();
            networkdManager.PlayerName = networkdManager.playerData.name = saveData.userName;
            networkdManager.playerData = new WorldNetworkManager.CreatePlayerMessage
            {
                Strength = saveData.stats[0],
                Intelligence = saveData.stats[1],
                Dexterity = saveData.stats[2],
                Constitution = saveData.stats[3]
            };
            networkdManager.playerData.position = new Vector3(saveData.positionX, saveData.positionY, saveData.positionZ);
        }

        public void SaveCharacter()
        {
            // Get the local player
            Player.PlayerController player = null;
            foreach (var item in FindObjectsOfType<Player.PlayerController>())
            {
                if (item.isLocalPlayer)
                {
                    player = item;
                    break;
                }
            }

            // Tell the save manager to save the data
            PlayerSaveManager.Save(player);
        }
    }
}