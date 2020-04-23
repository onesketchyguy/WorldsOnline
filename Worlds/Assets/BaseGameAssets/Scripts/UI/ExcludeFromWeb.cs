using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Worlds.UI
{
    public class ExcludeFromWeb : MonoBehaviour
    {
        private void OnEnable()
        {
#if UNITY_WEBGL
            gameObject.SetActive(false);
#endif
        }
    }
}