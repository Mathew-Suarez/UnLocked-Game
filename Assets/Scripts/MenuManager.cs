using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Scene Settings")]
    public string gameSceneName = "Level1";

    [Header("Continue Button")]
    public Button continueButton;

    void Start()
    {
        if (continueButton != null)
            continueButton.gameObject.SetActive(SaveSystem.SaveExists);
    }

    // ---------- NEW GAME ----------
    public void PlayGame()
    {
        SaveSystem.DeleteSave();

        if (GameManager.Instance != null)
            SceneManager.sceneLoaded -= GameManager.Instance.OnSceneLoaded;

        GameObject persistent = GameObject.Find("DontDestroyOnLoad");
        if (persistent != null)
            Destroy(persistent);

        if (GameManager.Instance != null)
            Destroy(GameManager.Instance.gameObject);

        SceneManager.LoadScene(gameSceneName);
    }

    // ---------- CONTINUE ----------
    public void ContinueGame()
    {
        SaveData data = SaveSystem.Load();
        if (data != null && !string.IsNullOrEmpty(data.sceneName))
        {
            SceneManager.LoadScene(data.sceneName);
        }
        else
        {
            Debug.Log("No save file found. Starting new game.");
            PlayGame();
        }
    }

    // ---------- QUIT ----------
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}