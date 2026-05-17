using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrialProgressManager : MonoBehaviour
{
    public static int CurrentProgress = 0;
    public static List<int> CompletedTrials = new List<int>();

    public string hostSceneName = "TRIAL_CHAMBERS";

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        LoadFromSave();
    }

    void LoadFromSave()
    {
        SaveData data = SaveSystem.Load();
        if (data != null)
        {
            CurrentProgress = data.trialProgress;
            CompletedTrials = data.completedTrials ?? new List<int>();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == hostSceneName)
        {
            LoadFromSave();   // refresh in case a trial was just completed
            EnablePortals(CurrentProgress);
        }
    }

    void EnablePortals(int completedTrials)
    {
        Debug.Log($"Enabling portals. Progress = {completedTrials}");
        GameObject container = GameObject.Find("TrialPortalsContainer");
        if (container == null)
        {
            Debug.LogError("TrialPortalsContainer not found!");
            return;
        }

        for (int i = 1; i <= 5; i++)
        {
            Transform child = container.transform.Find("TrialPortal" + i);
            if (child != null)
            {
                bool shouldBeActive = (i <= completedTrials + 1);
                child.gameObject.SetActive(shouldBeActive);
                Debug.Log($"TrialPortal{i} active = {shouldBeActive}");
            }
            else
            {
                Debug.LogWarning($"TrialPortal{i} not found under container!");
            }
        }
    }

    /// <summary>
    /// Call this when a trial puzzle is completed. Only advances progress if it's the next trial in order.
    /// </summary>
    public void CompleteTrial(int trialNumber)
    {
        // Already completed?
        if (CompletedTrials.Contains(trialNumber))
        {
            Debug.Log($"Trial {trialNumber} already completed.");
            return;
        }

        // Only allow completing the trial that is exactly next in sequence
        if (trialNumber != CurrentProgress + 1)
        {
            Debug.LogWarning($"Cannot complete trial {trialNumber}. Next expected trial is {CurrentProgress + 1}.");
            return;
        }

        CompletedTrials.Add(trialNumber);
        CurrentProgress = trialNumber;   // progress = the highest completed trial

        // Save
        SaveData data = SaveSystem.Load() ?? new SaveData();
        data.trialProgress = CurrentProgress;
        data.completedTrials = CompletedTrials;
        SaveSystem.Save(data);

        Debug.Log($"Trial {trialNumber} completed. Progress now: {CurrentProgress}");

        // Return to host scene
        if (GameManager.Instance != null)
            GameManager.Instance.LoadSceneWithFade(hostSceneName);
        else
            SceneManager.LoadScene(hostSceneName);
    }

    public void ResetAllProgress()
    {
        CurrentProgress = 0;
        CompletedTrials.Clear();

        SaveData data = SaveSystem.Load() ?? new SaveData();
        data.trialProgress = 0;
        data.completedTrials = CompletedTrials;
        SaveSystem.Save(data);

        Debug.Log("Trial progress reset to 0");
    }
}