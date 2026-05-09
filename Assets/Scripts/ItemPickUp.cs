using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [Header("Settings")]
    public float pickupDelay = 0.6f;
    private float spawnTime;

    void Start()
    {
        spawnTime = Time.time;

        // The "Pop" effect
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 jumpForce = new Vector2(Random.Range(-0.5f, 0.5f), 1.5f);
            rb.AddForce(jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the Player touched it AND if the delay is over
        if (other.CompareTag("Player") && Time.time > spawnTime + pickupDelay)
        {
            Collect();
        }
    }

    void Collect()
    {
        // 1. Get the Sprite from the SpriteRenderer on this object
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr != null && InventoryManager.instance != null)
        {
            // 2. Send the sprite to the UI
            InventoryManager.instance.UpdateSlot(sr.sprite);

            Debug.Log("Item Picked Up!");

            // 3. Destroy the physical item
            Destroy(gameObject);
        }
    }
}