using UnityEngine;
using UnityEngine.SceneManagement;

namespace WorldsUI
{
    public class MainMenuManager : MonoBehaviour
    {
        public void LoadScene(string sceneToLoad)
        {
            SceneManager.LoadScene(sceneToLoad); // Replace with loading screen.
        }

        public void QuitApp() => Application.Quit();
    }
}