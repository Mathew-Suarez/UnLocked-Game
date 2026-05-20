using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("UI Elements")]
    public GameObject storyPanel;
    public TextMeshProUGUI storyText;
    public Button nextButton;           // Optional – can be wired to AdvanceDialogue()

    [Header("Typing Effect")]
    [SerializeField] private float typingSpeed = 0.04f;   // seconds per character

    private string[] currentLines;
    private int currentLineIndex = 0;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string currentFullLine;      // The complete line being typed

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>Call this from an NPC to start a story.</summary>
    public void StartStory(string[] lines)
    {
        if (lines == null || lines.Length == 0)
            return;

        currentLines = lines;
        currentLineIndex = 0;
        storyPanel.SetActive(true);
        DisplayCurrentLine();
    }

    /// <summary>Starts typing the current line.</summary>
    private void DisplayCurrentLine()
    {
        if (currentLineIndex >= currentLines.Length)
        {
            EndStory();
            return;
        }

        // Stop any previous typing
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        currentFullLine = currentLines[currentLineIndex];
        storyText.text = "";
        isTyping = true;
        typingCoroutine = StartCoroutine(TypeLine(currentFullLine));
    }

    /// <summary>Types the line one character at a time.</summary>
    private IEnumerator TypeLine(string line)
    {
        foreach (char c in line)
        {
            storyText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    /// <summary>Call this from a UI button or the interact key when dialogue is open.</summary>
    public void AdvanceDialogue()
    {
        // If currently typing, complete the line immediately
        if (isTyping)
        {
            CompleteTyping();
            return;
        }

        // Otherwise move to the next line
        currentLineIndex++;
        DisplayCurrentLine();
    }

    /// <summary>Instantly shows the full line and stops typing.</summary>
    private void CompleteTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        storyText.text = currentFullLine;
        isTyping = false;
    }

    /// <summary>Ends the dialogue and hides the panel.</summary>
    public void EndStory()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        storyPanel.SetActive(false);
        storyText.text = "";
        isTyping = false;
    }
}