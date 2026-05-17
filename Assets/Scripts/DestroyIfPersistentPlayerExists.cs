using UnityEngine;

public class DestroyIfPersistentPlayerExists : MonoBehaviour
{
    void Awake()
    {
        // If there's already a persistent Player (not this one), destroy this object
        PersistentPlayer persistent = FindObjectOfType<PersistentPlayer>();
        if (persistent != null && persistent.gameObject != gameObject)
        {
            Destroy(gameObject);
        }
    }
}