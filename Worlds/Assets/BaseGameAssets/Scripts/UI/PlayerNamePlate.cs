using UnityEngine;
using UnityEngine.UI;

namespace World.Player
{
    [RequireComponent(typeof(Text))]
    public class PlayerNamePlate : MonoBehaviour
    {
        public PlayerController player;

        private void Start()
        {
            // Set the text of our name plate
            if (player == null)
                GetComponent<Text>().text = "";
            else
                GetComponent<Text>().text = $"{player.playerName}";

            // Then clean up this component by removing it
            Destroy(this);
        }
    }
}