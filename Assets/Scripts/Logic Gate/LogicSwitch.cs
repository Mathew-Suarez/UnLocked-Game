using UnityEngine;
using TMPro;   // Required for TextMeshPro

public class LogicSwitch : MonoBehaviour, IInteractable, ILogicInput
{
    [Header("State")]
    public bool isOn = false;

    [Header("Switch Visuals")]
    public SpriteRenderer bodySprite;
    public Sprite offSprite;
    public Sprite onSprite;

    public GameObject lightIndicator;
    public Color offColor = Color.red;
    public Color onColor = Color.green;

    [Header("External Target Sprite (optional)")]
    public SpriteRenderer targetSpriteRenderer;
    public Sprite targetOffSprite;
    public Sprite targetOnSprite;

    [Header("Number Display (optional)")]
    public TextMeshProUGUI numberText;      // Assign a TextMeshPro UI element

    private SpriteRenderer lightRenderer;

    public bool IsOn => isOn;

    void Start()
    {
        if (lightIndicator != null)
            lightRenderer = lightIndicator.GetComponent<SpriteRenderer>();
        UpdateVisuals();
    }

    public void Interact(Transform player) => Toggle();

    public void Toggle()
    {
        isOn = !isOn;
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        // Own body sprite
        if (bodySprite != null)
            bodySprite.sprite = isOn ? onSprite : offSprite;

        // Own light indicator
        if (lightRenderer != null)
            lightRenderer.color = isOn ? onColor : offColor;

        // External target sprite
        if (targetSpriteRenderer != null)
            targetSpriteRenderer.sprite = isOn ? targetOnSprite : targetOffSprite;

        // Number display (TextMeshPro)
        if (numberText != null)
            numberText.text = isOn ? "1" : "0";
    }
}