using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
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