using UnityEngine;

public class Door : MonoBehaviour
{
    public void Open()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        var sr = GetComponent<SpriteRenderer>();
        if (sr) sr.enabled = false;
    }
}