using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    [RequireComponent(typeof(Text))]
    public class PlayerNamePlate : MonoBehaviour
    {
        public PlayerController player;

        private void Start()
        {
            GetComponent<Text>().text = $"{player.playerName}";
        }
    }
}