using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public TMP_Text healthtext;
    public Animator healthTextAnim;

    [Header("Regeneration")]
    public float regenerationRate = 1f;
    public float regenerationDelay = 3f;
    private float lastDamageTime = -999f;
    private Coroutine regenCoroutine;

    private Vector3 respawnPosition;
    private bool hasRespawnPoint = false;

    private void Start()
    {
        respawnPosition = transform.position;
        hasRespawnPoint = true;
        UpdateHealthText();
        regenCoroutine = StartCoroutine(RegenRoutine());
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

        healthTextAnim.Play("TextUpdate");
        UpdateHealthText();

        if (amount < 0)
            lastDamageTime = Time.time;

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

    IEnumerator RegenRoutine()
    {
        while (true)
        {
            if (currentHealth < maxHealth && Time.time > lastDamageTime + regenerationDelay)
            {
                currentHealth += Mathf.CeilToInt(regenerationRate * Time.deltaTime);
                currentHealth = Mathf.Min(currentHealth, maxHealth);
                UpdateHealthText();
            }
            yield return null;
        }
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