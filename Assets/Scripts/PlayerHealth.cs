using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public TMP_Text healthtext;
    public Animator healthTextAnim;

    private void Start()
    {
        healthtext.text = "Hp: " + currentHealth + " / " +  maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        healthTextAnim.Play("TextUpdate");
        healthtext.text = "Hp: " + currentHealth + " / " + maxHealth;

        if (currentHealth <= 0)
        {   
            currentHealth = 0;
            healthtext.text = "Hp: " + currentHealth + " / " + maxHealth;
            gameObject.SetActive(false);
        }
    }
}
