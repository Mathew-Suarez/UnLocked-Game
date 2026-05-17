using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [Header("Visual Feedback")]
    public SpriteRenderer spriteRenderer;   // drag the sprite renderer here
    public Color activeColor = Color.green;
    public Color inactiveColor = Color.gray;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Tell the player’s health/respawn system to use this point
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.SetRespawnPoint(transform.position);
                // Optional: change colour to show it’s active
                if (spriteRenderer != null)
                    spriteRenderer.color = activeColor;
            }
        }
    }

    // Reset colour when the player leaves (optional)
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && spriteRenderer != null)
            spriteRenderer.color = inactiveColor;
    }
}