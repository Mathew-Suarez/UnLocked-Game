using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour, IInteractable
{
    public string sceneToLoad;
    public Image fadeImage;
    public float fadeDuration = 1f;

    // Static reference so GameManager can clear it later
    public static Image StaticFadeImage;

    private void Awake()
    {
        if (fadeImage != null)
            StaticFadeImage = fadeImage;
    }

    public void Interact(Transform player)
    {
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("[Portal] Scene name is empty!");
            return;
        }

        if (fadeImage != null)
            StartCoroutine(FadeAndLoad());
        else
            SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator FadeAndLoad()
    {
        // Enable image and make it block clicks
        fadeImage.gameObject.SetActive(true);
        fadeImage.raycastTarget = true;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeImage.color = Color.black;
        SceneManager.LoadScene(sceneToLoad);
    }
}