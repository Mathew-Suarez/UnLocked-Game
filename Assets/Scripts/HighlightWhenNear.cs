using UnityEngine;

public class HighlightWhenNear : MonoBehaviour
{
    [Header("Trigger Detection")]
    [Tooltip("The trigger collider on this object.")]
    public Collider2D triggerCollider;

    [Header("Highlight Mode")]
    public HighlightMode mode = HighlightMode.TintSprite;

    [Header("Tint Settings (if mode = TintSprite)")]
    public SpriteRenderer targetRenderer;      // The sprite to tint
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    [Header("Separate Highlight Object (if mode = EnableObject)")]
    public GameObject highlightObject;         // A child outline/glow that toggles on/off

    public enum HighlightMode { TintSprite, EnableObject }

    private bool isPlayerNear = false;

    private void Start()
    {
        if (triggerCollider == null)
            triggerCollider = GetComponent<Collider2D>();

        ApplyNormal();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            ApplyHighlight();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            ApplyNormal();
        }
    }

    private void ApplyHighlight()
    {
        switch (mode)
        {
            case HighlightMode.TintSprite:
                if (targetRenderer != null)
                    targetRenderer.color = highlightColor;
                break;

            case HighlightMode.EnableObject:
                if (highlightObject != null)
                    highlightObject.SetActive(true);
                break;
        }
    }

    private void ApplyNormal()
    {
        switch (mode)
        {
            case HighlightMode.TintSprite:
                if (targetRenderer != null)
                    targetRenderer.color = normalColor;
                break;

            case HighlightMode.EnableObject:
                if (highlightObject != null)
                    highlightObject.SetActive(false);
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (triggerCollider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, triggerCollider.bounds.size);
        }
    }
}