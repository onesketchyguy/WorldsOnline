using UnityEngine;

namespace Worlds.UI
{
    public class BillboardFaceCamera : MonoBehaviour
    {
        private new Transform camera;

        private void LateUpdate()
        {
            if (camera == null)
            {
                camera = Camera.main.transform;
                return;
            }

            transform.LookAt(transform.position + camera.forward);
        }
    }
}