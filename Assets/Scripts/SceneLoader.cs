using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene to Load")]
    public string sceneName;
    public int sceneIndex = -1;

    [Header("Delay (optional)")]
    public float delayBeforeLoad = 1f;          // seconds to wait after fading, before loading

    [Header("Fade Overlay (optional)")]
    public Image fadeImage;                     // drag the persistent FadeImage here
    public float fadeDuration = 0.5f;           // how long the fade‑out takes

    private void Awake()
    {
        // If a fade image is assigned and no other script has set the static reference yet,
        // make it available to the GameManager for the fade‑in after loading.
        if (fadeImage != null && Portal.StaticFadeImage == null)
            Portal.StaticFadeImage = fadeImage;
    }

    /// <summary>Call this from a UnityEvent (e.g., button OnClick).</summary>
    public void LoadSceneByName()
    {
        if (!string.IsNullOrEmpty(sceneName))
            StartCoroutine(FadeOutAndLoad(sceneName));
    }

    public void LoadSceneByIndex()
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
            StartCoroutine(FadeOutAndLoad(sceneIndex));
    }

    public void LoadScene(string name)
    {
        if (!string.IsNullOrEmpty(name))
            StartCoroutine(FadeOutAndLoad(name));
    }

    private IEnumerator FadeOutAndLoad(string scene)
    {
        yield return FadeOutRoutine();
        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene(scene);
    }

    private IEnumerator FadeOutAndLoad(int index)
    {
        yield return FadeOutRoutine();
        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene(index);
    }

    private IEnumerator FadeOutRoutine()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.raycastTarget = true;    // block input during transition

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
        // If no fade image is assigned, this yields nothing – just a no‑op.
        yield break;
    }
}