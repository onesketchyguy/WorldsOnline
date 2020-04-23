using UnityEngine;

namespace Worlds.UI
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