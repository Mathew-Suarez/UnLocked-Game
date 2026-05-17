using UnityEngine;

public class SceneSpawnPoint : MonoBehaviour
{
    void Start()
    {
        // Find the persistent player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Move the player to this spawn point
            player.transform.position = transform.position;
        }
    }
}