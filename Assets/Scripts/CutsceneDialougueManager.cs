using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CutsceneDialogueManager : MonoBehaviour
{
    public static CutsceneDialogueManager instance;

    [Header("UI References")]
    public GameObject cutscenePanel;        // Black panel (fullscreen, initially disabled)
    public TextMeshProUGUI cutsceneText;    // Text for the dialogue line
    public Button nextButton;               // "Next" button
    public Button skipButton;               // "Skip" button

    [Header("Typing Settings")]
    [SerializeField] private float typingSpeed = 0.04f;   // seconds per character

    private string[] currentLines;
    private int currentLineIndex = 0;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string currentFullLine;
    private string pendingSceneName;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Hide the panel at startup
        if (cutscenePanel != null)
            cutscenePanel.SetActive(false);

        // Wire up buttons
        if (nextButton != null)
            nextButton.onClick.AddListener(AdvanceCutscene);
        if (skipButton != null)
            skipButton.onClick.AddListener(SkipCutscene);
    }

    /// <summary>
    /// Start a cutscene with the given lines. After the cutscene ends,
    /// the specified scene will be loaded.
    /// </summary>
    public void PlayCutscene(string[] lines, string sceneName)
    {
        // If no lines, just load the scene directly
        if (lines == null || lines.Length == 0)
        {
            if (!string.IsNullOrEmpty(sceneName))
                SceneManager.LoadScene(sceneName);
            return;
        }

        currentLines = lines;
        currentLineIndex = 0;
        pendingSceneName = sceneName;

        // Show the cutscene panel
        if (cutscenePanel != null)
            cutscenePanel.SetActive(true);

        DisplayCurrentLine();
    }

    /// <summary>
    /// Is a cutscene currently playing?
    /// </summary>
    public bool IsPlaying => cutscenePanel != null && cutscenePanel.activeSelf;

    // ---- Internal logic ----

    private void DisplayCurrentLine()
    {
        if (currentLineIndex >= currentLines.Length)
        {
            EndCutscene();
            return;
        }

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        currentFullLine = currentLines[currentLineIndex];
        if (cutsceneText != null)
            cutsceneText.text = "";
        isTyping = true;
        typingCoroutine = StartCoroutine(TypeLine(currentFullLine));
    }

    private IEnumerator TypeLine(string line)
    {
        foreach (char c in line)
        {
            if (cutsceneText != null)
                cutsceneText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    public void AdvanceCutscene()
    {
        if (isTyping)
        {
            // Complete the current line immediately
            CompleteTyping();
        }
        else
        {
            // Move to the next line
            currentLineIndex++;
            DisplayCurrentLine();
        }
    }

    private void CompleteTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (cutsceneText != null)
            cutsceneText.text = currentFullLine;
        isTyping = false;
    }

    public void SkipCutscene()
    {
        // Immediately end the cutscene and load the target scene
        EndCutscene();
    }

    private void EndCutscene()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        isTyping = false;

        if (cutscenePanel != null)
            cutscenePanel.SetActive(false);

        if (!string.IsNullOrEmpty(pendingSceneName))
            SceneManager.LoadScene(pendingSceneName);
    }
}