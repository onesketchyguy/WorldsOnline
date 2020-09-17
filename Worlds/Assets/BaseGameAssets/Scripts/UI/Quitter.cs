using UnityEngine;

public class Quitter : MonoBehaviour
{
    public void QuitApp() 
    {
#if UNITY_EDITOR
        Debug.Break();
#endif

        Application.Quit();
    }
}
