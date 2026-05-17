using UnityEngine;

public class TrialCompleter : MonoBehaviour
{
    public string hostSceneName = "TRIAL_CHAMBERS";
    public int trialNumber = 1;   // set in Inspector: 1,2,3,4,5

    public void Complete()
    {
        TrialProgressManager manager = FindObjectOfType<TrialProgressManager>();
        if (manager != null)
            manager.CompleteTrial(trialNumber);
        else
            Debug.LogError("No TrialProgressManager found in scene!");
    }
}