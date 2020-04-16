using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObjectOnKeyPress : MonoBehaviour
{
    public GameObject objectToToggle;
    public string Key = "Cancel";

    [Tooltip("The state of the object when the scene starts.")]
    public bool defaultState = true;

    private void Start()
    {
        if (!objectToToggle.activeSelf == defaultState)
            Toggle();
    }

    private void Update()
    {
        if (Input.GetButtonDown(Key))
            Toggle();
    }

    public void Toggle()
    {
        objectToToggle.SetActive(!objectToToggle.activeSelf);
    }
}