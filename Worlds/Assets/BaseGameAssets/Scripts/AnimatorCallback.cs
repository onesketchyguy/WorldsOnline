using UnityEngine;

namespace Worlds.Effects
{
    public class AnimatorCallback : MonoBehaviour
    {
        public UnityEngine.Events.UnityEvent[] actions;

        public void InvokeMethod(int index)
        {
            // Call an element
            Debug.Log($"{gameObject.name}.Animator InvokedMethod {index}");

            if (index < actions.Length)
            {
                actions[index].Invoke();
            }
        }
    }
}