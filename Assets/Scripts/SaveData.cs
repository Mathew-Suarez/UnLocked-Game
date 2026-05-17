using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public string sceneName;
    public int playerHealth;
    public int playerMaxHealth;
    public float[] playerPosition; // stores x, y, z
    public List<InventoryItemData> inventoryItems;
    // Add more fields later (e.g., bool boss1Defeated;)
    public int trialProgress = 0;        // keep for backward compatibility
    public List<int> completedTrials = new List<int>();   // NEW
}

[Serializable]
public class InventoryItemData
{
    public string itemName;
    public int quantity;
}