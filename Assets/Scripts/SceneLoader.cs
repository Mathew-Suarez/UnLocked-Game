using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Choose one of these methods to load a scene
    public string sceneName;     // e.g. "Level2"
    public int sceneIndex = -1;  // e.g. 1

    // 1. Load by scene name
    public void LoadSceneByName()
    {
        if (!string.IsNullOrEmpty(sceneName))
            SceneManager.LoadScene(sceneName);
    }

    // 2. Load by build index
    public void LoadSceneByIndex()
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(sceneIndex);
    }

    // 3. Call this from anywhere, passing a string
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}