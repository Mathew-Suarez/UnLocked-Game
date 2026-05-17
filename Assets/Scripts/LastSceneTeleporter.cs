using UnityEngine;
using UnityEngine.SceneManagement;

public class LastSceneTeleporter : MonoBehaviour
{
    // Call this whenever you want to remember the current scene
    public static void SaveCurrentScene()
    {
        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
        Debug.Log("Last scene saved: " + SceneManager.GetActiveScene().name);
    }

    // Call this from a button to load the last saved scene
    public void LoadLastScene()
    {
        if (PlayerPrefs.HasKey("LastScene"))
        {
            string sceneName = PlayerPrefs.GetString("LastScene");
            Debug.Log("Loading last scene: " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("No last scene saved! Load a default scene or show a message.");
            // Optional: load a fallback scene
            // SceneManager.LoadScene("MainMenu");
        }
    }
}