using UnityEngine;
using UnityEngine.UI;       // Required for Image

public class PlayerInteracter : MonoBehaviour
{
    private IInteractable currentInteractable;

    [Header("Button Highlight")]
    public Image interactButtonImage;          // The Image component of your InteractButton
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    private void Awake()
    {
        // If not manually assigned, try to find the button by name
        if (interactButtonImage == null)
        {
            GameObject btnObj = GameObject.Find("InteractButton");
            if (btnObj != null)
                interactButtonImage = btnObj.GetComponent<Image>();
        }

        // Set initial normal colour
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