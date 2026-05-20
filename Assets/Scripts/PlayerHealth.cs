using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public TMP_Text healthtext;
    public Animator healthTextAnim;

    private Vector3 respawnPosition;
    private bool hasRespawnPoint = false;

    private void Start()
    {
        respawnPosition = transform.position;
        hasRespawnPoint = true;
        UpdateHealthText();
    }

    public void SetRespawnPoint(Vector3 newPoint)
    {
        respawnPosition = newPoint;
        hasRespawnPoint = true;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthTextAnim != null)
            healthTextAnim.Play("TextUpdate");

        UpdateHealthText();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            UpdateHealthText();
            Die();
        }
    }

    void UpdateHealthText()
    {
        if (healthtext != null)
            healthtext.text = "Hp: " + currentHealth + " / " + maxHealth;
    }

    void Die()
    {
        currentHealth = maxHealth;
        UpdateHealthText();

        if (hasRespawnPoint)
            transform.position = respawnPosition;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = Vector2.zero;

        gameObject.SetActive(true);
    }
}
