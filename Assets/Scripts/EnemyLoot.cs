using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    [Header("Drop Settings")]
    public GameObject itemToDrop;
    [Range(0f, 100f)]
    public float dropChance = 50f; 

    
    public void DropItem()
    {
        Debug.Log("DropItem() was called!"); 
        float randomRoll = Random.Range(0f, 100f);

        if (randomRoll <= dropChance)
        {
            Debug.Log("Roll Succeeded! Spawning " + itemToDrop.name);
            Instantiate(itemToDrop, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Roll Failed. No drop this time.");
        }
    }

    // Example: Triggering drop when this object is destroyed
    private void OnDestroy()
    {
        // Safety check to ensure we don't spawn items when stopping the game
        if (!gameObject.scene.isLoaded) return;

        DropItem();
    }
}
