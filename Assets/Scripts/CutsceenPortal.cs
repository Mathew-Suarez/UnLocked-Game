using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CutscenePortal : MonoBehaviour, IInteractable
{
    [Header("Scene")]
    public string sceneToLoad;

    [Header("Cutscene Lines")]
    [TextArea(3, 10)]
    public string[] cutsceneLines;

    [Header("Fade (optional)")]
    public Image fadeImage;                 // drag the persistent FadeImage here
    public float fadeDuration = 0.5f;

    [Header("Auto-trigger")]
    public bool triggerOnCollision = false; // if true, player only needs to walk into the trigger

    private bool hasTriggered = false;

    public void Interact(Transform player)
    {
        if (!hasTriggered)
            StartCutscene();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerOnCollision && other.CompareTag("Player") && !hasTriggered)
        {
            StartCutscene();
        }
    }

    private void StartCutscene()
    {
        if (hasTriggered) return;
        hasTriggered = true;

        if (fadeImage != null)
        {
            StartCoroutine(FadeOutThenCutscene());
        }
        else
        {
            // No fade – immediately start cutscene
            if (CutsceneDialogueManager.instance != null)
                CutsceneDialogueManager.instance.PlayCutscene(cutsceneLines, sceneToLoad);
            else
                Debug.LogError("CutsceneDialogueManager instance not found!");
        }
    }

    private IEnumerator FadeOutThenCutscene()
    {
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

        // Now start the cutscene dialogue
        if (CutsceneDialogueManager.instance != null)
            CutsceneDialogueManager.instance.PlayCutscene(cutsceneLines, sceneToLoad);
    }
}