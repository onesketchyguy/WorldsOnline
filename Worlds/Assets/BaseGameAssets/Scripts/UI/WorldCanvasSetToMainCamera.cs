using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Canvas))]
    public class WorldCanvasSetToMainCamera : MonoBehaviour
    {
        private void Update()
        {
            var canv = GetComponent<Canvas>();

            if (canv.worldCamera == null)
            {
                canv.worldCamera = Camera.main;
                Destroy(this);
            }
        }
    }
}