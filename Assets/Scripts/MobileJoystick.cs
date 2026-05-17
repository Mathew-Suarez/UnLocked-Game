using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    // Static instance so any script can find the joystick instantly
    public static MobileJoystick instance;

    public RectTransform handle;
    public float maxRadius = 90f;

    private Vector2 input = Vector2.zero;

    public float Horizontal => input.x;
    public float Vertical => input.y;

    void Awake()
    {
        // If there's already an instance and it's not us, destroy this component
        if (instance != null && instance != this)
        {
            Destroy(this);   // remove the joystick script from this object
            return;
        }
        instance = this;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPos;
        RectTransform baseRect = handle.parent as RectTransform;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                baseRect,
                eventData.position,
                eventData.pressEventCamera,
                out localPos))
        {
            Vector2 clampedPos = Vector2.ClampMagnitude(localPos, maxRadius);
            handle.anchoredPosition = clampedPos;
            input = clampedPos / maxRadius;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handle.anchoredPosition = Vector2.zero;
        input = Vector2.zero;
    }
}