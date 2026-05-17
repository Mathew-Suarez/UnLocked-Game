using UnityEngine;
using UnityEngine.SceneManagement;

public class FullReset : MonoBehaviour
{
    [Header("Scene to load after reset")]
    public string firstGameScene = "Level1";   // or "MainMenu"

    public void ResetEverything()
    {
        // 1. Clear inventory (if present)
        if (InventoryManager.instance != null)
            InventoryManager.instance.ClearInventory();

        // 2. Reset trial progress
        TrialProgressManager manager = FindObjectOfType<TrialProgressManager>();
        if (manager != null)
            manager.ResetAllProgress();
        // Reset player health
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.currentHealth = ph.maxHealth;
                // Update the UI text directly (since we're setting the variable, not using ChangeHealth)
                if (ph.healthtext != null)
                    ph.healthtext.text = "Hp: " + ph.currentHealth + " / " + ph.maxHealth;
            }
        }

        // 3. Delete save file
        SaveSystem.DeleteSave();

        // 6. Load a fresh scene
        SceneManager.LoadScene(firstGameScene);
    }
}