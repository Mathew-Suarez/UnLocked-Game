using UnityEngine;
using UnityEngine.UI;

public class PlayerInteracter : MonoBehaviour
{
    private IInteractable currentInteractable;

    [Header("Button Highlight")]
    public Image interactButtonImage;
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    private void Awake()
    {
        if (interactButtonImage == null)
        {
            GameObject btnObj = GameObject.Find("InteractButton");
            if (btnObj != null)
                interactButtonImage = btnObj.GetComponent<Image>();
        }
        if (interactButtonImage != null)
            interactButtonImage.color = normalColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            currentInteractable = interactable;
            HighlightButton(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable) && interactable == currentInteractable)
        {
            currentInteractable = null;
            HighlightButton(false);
        }
    }

    public void Interact()
    {
        // 1. If a cutscene is playing, advance it
        if (CutsceneDialogueManager.instance != null && CutsceneDialogueManager.instance.IsPlaying)
        {
            CutsceneDialogueManager.instance.AdvanceCutscene();
            return;
        }

        // 2. If the normal dialogue panel is open, advance it
        if (DialogueManager.instance != null && DialogueManager.instance.storyPanel.activeSelf)
        {
            DialogueManager.instance.AdvanceDialogue();
            return;
        }

        // 3. Otherwise, interact with the object in front of the player
        if (currentInteractable != null)
            currentInteractable.Interact(transform);
        else
            Debug.Log("Nothing to interact with.");
    }

    private void HighlightButton(bool highlight)
    {
        if (interactButtonImage != null)
            interactButtonImage.color = highlight ? highlightColor : normalColor;
    }
}