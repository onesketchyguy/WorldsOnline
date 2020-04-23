using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnObjectBecomeActiveDoEvent : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent @event;

    private void OnEnable()
    {
        @event.Invoke();
    }
}