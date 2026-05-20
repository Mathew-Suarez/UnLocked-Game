using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Scene Settings")]
    public string gameSceneName = "Level1";   // your first game scene

    [Header("Continue Button")]
    public Button continueButton;

    [Header("Fade Overlay")]
    public Image fadeImage;                  // Drag the persistent FadeImage here
    public float fadeDuration = 0.5f;        // How long the fade takes

    void Start()
    {
        if (continueButton != null)
            continueButton.gameObject.SetActive(SaveSystem.SaveExists);
    }

    // ---------- NEW GAME (with fade) ----------
    public void PlayGame()
    {
        StartCoroutine(FadeAndStartNewGame());
    }

    private IEnumerator FadeAndStartNewGame()
    {
        // 1. Activate the fade image and fade to black
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.raycastTarget = true;        // block clicks during transition

            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Clamp01(timer / fadeDuration);
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
            fadeImage.color = Color.black;
        }

        // 2. Delete any old save file
        SaveSystem.DeleteSave();

        // 3. Unsubscribe GameManager from sceneLoaded to avoid errors
        if (GameManager.Instance != null)
            SceneManager.sceneLoaded -= GameManager.Instance.OnSceneLoaded;

        // 4. Destroy the persistent DontDestroyOnLoad parent (and all its children: Player, Canvas, etc.)
        GameObject persistent = GameObject.Find("DontDestroyOnLoad");
        if (persistent != null)
            Destroy(persistent);

        // 5. Destroy the GameManager singleton
        if (GameManager.Instance != null)
            Destroy(GameManager.Instance.gameObject);

        // 6. Load the first game scene
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