using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager created and set to DontDestroyOnLoad");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Duplicate GameManager destroyed");
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainMenu")
            LastSceneTeleporter.SaveCurrentScene();

        Debug.Log("Scene loaded: " + scene.name);

        if (Portal.StaticFadeImage != null)
            StartCoroutine(FadeIn(Portal.StaticFadeImage));
    }

    IEnumerator FadeIn(Image fadeImage)
    {
        fadeImage.raycastTarget = true;
        float timer = 0f;
        float duration = 0.5f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(timer / duration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeImage.color = Color.clear;
        fadeImage.raycastTarget = false;
        fadeImage.gameObject.SetActive(false);
    }

    // ---------- SAVE PROGRESS ----------
    public void SaveProgress()
    {
        SaveData data = new SaveData();
        data.sceneName = SceneManager.GetActiveScene().name;
        data.trialProgress = TrialProgressManager.CurrentProgress;   // requires TrialProgressManager script

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                data.playerHealth = ph.currentHealth;
                data.playerMaxHealth = ph.maxHealth;
            }
            Vector3 pos = player.transform.position;
            data.playerPosition = new float[] { pos.x, pos.y, pos.z };
        }

        if (InventoryManager.instance != null)
        {
            var invData = InventoryManager.instance.GetSaveData();
            data.inventoryItems = new List<InventoryItemData>();
            if (invData != null)
                data.inventoryItems.Add(invData);
        }

        SaveSystem.Save(data);
        Debug.Log("Game saved!");
    }
    public void LoadSceneWithFade(string sceneName)
    {
        if (Portal.StaticFadeImage != null)
            StartCoroutine(FadeOutAndLoad(sceneName));
        else
            SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeOutAndLoad(string sceneName)
    {
        Image fadeImage = Portal.StaticFadeImage;
        fadeImage.gameObject.SetActive(true);
        fadeImage.raycastTarget = true;

        float timer = 0f;
        float duration = 0.5f;   // match your fade duration
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / duration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeImage.color = Color.black;
        SceneManager.LoadScene(sceneName);
    }
}