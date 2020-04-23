using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Worlds.UI
{
    using Saves;

    public class PlayerSavesUI : MonoBehaviour
    {
        public UnityEngine.Events.UnityEvent OnLoadEvent;
        public GameObject buttonPrefab;

        public void CreateButtons(Transform parent)
        {
            //PlayerSaveManager.LoadAll()

            var dat = new PlayerSaveManager.SaveData[] {
                PlayerSaveManager.Load("Forrest")
            };
            if (dat == null || dat.Length < 1) return;

            foreach (var save in dat)
                CreateButton(save, parent);
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
            networkdManager.PlayerName = saveData.userName;
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