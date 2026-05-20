using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    public int damage = 1;

    [Header("Continuous Damage Settings")]
    [Tooltip("How many seconds between each damage tick while touching.")]
    public float damageInterval = 1.0f;

    private float damageTimer;
    private bool isTouchingPlayer = false;
    private PlayerHealth playerHealthReference;

    void Update()
    {
        // 1. If we are touching the player, continuously count down time every frame
        if (isTouchingPlayer && playerHealthReference != null)
        {
            damageTimer += Time.deltaTime;

            // 2. Trigger damage again once the interval time is reached
            if (damageTimer >= damageInterval)
            {
                playerHealthReference.ChangeHealth(-damage);
                damageTimer = 0f; // Reset timer for the next interval
                Debug.Log("Continuous damage dealt to player!");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealthReference = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealthReference != null)
            {
                // 3. Deal damage immediately upon the first touch
                playerHealthReference.ChangeHealth(-damage);

                // 4. Start tracking continuous damage
                isTouchingPlayer = true;
                damageTimer = 0f;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 5. Stop dealing continuous damage when player walks away
            isTouchingPlayer = false;
            playerHealthReference = null;
            damageTimer = 0f;
        }
    }
}
