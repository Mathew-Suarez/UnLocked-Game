using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("UI Elements")]
    public GameObject storyPanel;
    public TextMeshProUGUI storyText;
    public Button nextButton; // optional if you want to auto-wire, else use onClick event

    private string[] currentLines;
    private int currentLineIndex = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Call this from the NPC to start a story
    public void StartStory(string[] lines)
    {
        if (lines == null || lines.Length == 0)
            return;

        currentLines = lines;
        currentLineIndex = 0;
        storyPanel.SetActive(true);
        ShowLine();
    }

    void ShowLine()
    {
        if (currentLineIndex < currentLines.Length)
        {
            storyText.text = currentLines[currentLineIndex];
        }
        else
        {
            EndStory();
        }
    }

    public void NextLine()
    {
        currentLineIndex++;
        ShowLine();
    }

    public void EndStory()
    {
        storyPanel.SetActive(false);
        storyText.text = "";
    }
}