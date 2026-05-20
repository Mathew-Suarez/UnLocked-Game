using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [Header("UI Slot References (Must drag exactly 4 elements)")]
    [Tooltip("Drag your 4 visual child Icon Image components here in order.")]
    public List<Image> itemIconsInSlots = new List<Image>();

    [Tooltip("Drag your 4 text quantities here matching the order above.")]
    public List<TextMeshProUGUI> quantityTexts = new List<TextMeshProUGUI>();

    [Header("Inventory Data Storage (Leave Empty in Inspector)")]
    public List<InventoryItemData> slotsData = new List<InventoryItemData>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        // Securely initialize 4 empty backend data blocks
        slotsData.Clear();
        for (int i = 0; i < 4; i++)
        {
            slotsData.Add(new InventoryItemData());
        }
    }

    private void Start()
    {
        UpdateAllVisualSlots();
    }

    // Handles picking up or trading items safely
    public void UpdateSlot(Sprite newSprite)
    {
        if (newSprite == null) return;
        string pickedItemName = newSprite.name;

        // 1. Check if the item already exists in one of the slots to stack it
        for (int i = 0; i < 4; i++)
        {
            if (string.Equals(slotsData[i].itemName, pickedItemName, System.StringComparison.OrdinalIgnoreCase))
            {
                slotsData[i].quantity++;
                UpdateVisualSlot(i, newSprite);
                Debug.Log($"Stacked item! {pickedItemName} quantity is now {slotsData[i].quantity}");
                return;
            }
        }

        // 2. Find a slot that has a quantity of 0 AND an empty name string
        for (int i = 0; i < 4; i++)
        {
            if (slotsData[i].quantity == 0 && string.IsNullOrEmpty(slotsData[i].itemName))
            {
                slotsData[i].itemName = pickedItemName;
                slotsData[i].quantity = 1;
                UpdateVisualSlot(i, newSprite);
                Debug.Log($"Added new item to empty slot {i}: {pickedItemName}");
                return;
            }
        }

        Debug.LogWarning("Inventory is full! Cannot pick up: " + pickedItemName);
    }

    // Updates a single slot graphic and text immediately
    private void UpdateVisualSlot(int index, Sprite sprite)
    {
        if (index < 0 || index >= 4) return;
        if (index >= itemIconsInSlots.Count || index >= quantityTexts.Count) return;
        if (itemIconsInSlots[index] == null || quantityTexts[index] == null) return;

        if (slotsData[index].quantity > 0 && !string.IsNullOrEmpty(slotsData[index].itemName))
        {
            // If no sprite was passed, dynamically load it from the Resources/Sprites folder
            if (sprite == null)
            {
                sprite = Resources.Load<Sprite>("Sprites/" + slotsData[index].itemName);
            }

            if (sprite != null)
            {
                itemIconsInSlots[index].sprite = sprite;
                itemIconsInSlots[index].color = Color.white; // Ensure it is visible
            }

            // FIXED: Explicitly force the UI text component to rewrite with the new, reduced quantity value!
            quantityTexts[index].text = slotsData[index].quantity.ToString();
            quantityTexts[index].color = Color.white;
        }
        else
        {
            // Clear the slot visuals if the item is entirely gone
            itemIconsInSlots[index].sprite = null;
            itemIconsInSlots[index].color = new Color(1, 1, 1, 0); // Transparent
            quantityTexts[index].text = "";
        }
    }

    public void UpdateAllVisualSlots()
    {
        for (int i = 0; i < 4; i++)
        {
            UpdateVisualSlot(i, null);
        }
    }

    public void ClearInventory()
    {
        for (int i = 0; i < 4; i++)
        {
            slotsData[i].itemName = "";
            slotsData[i].quantity = 0;
            UpdateVisualSlot(i, null);
        }
    }

    public int GetItemQuantity(string itemName)
    {
        for (int i = 0; i < 4; i++)
        {
            if (string.Equals(slotsData[i].itemName, itemName, System.StringComparison.OrdinalIgnoreCase))
                return slotsData[i].quantity;
        }
        return 0;
    }

    public void RemoveItems(string itemName, int amountToRemove)
    {
        for (int i = 0; i < 4; i++)
        {
            if (string.Equals(slotsData[i].itemName, itemName, System.StringComparison.OrdinalIgnoreCase))
            {
                // Subtract only the cost price tag from backend data memory
                slotsData[i].quantity -= amountToRemove;

                if (slotsData[i].quantity <= 0)
                {
                    slotsData[i].itemName = "";
                    slotsData[i].quantity = 0;
                    UpdateVisualSlot(i, null); // Wipe UI slot cleanly
                }
                else
                {
                    // FIXED: We pass 'null' here, and the updated method above will fetch the 
                    // correct image while forcing the text number to reflect the fresh subtracted math!
                    UpdateVisualSlot(i, null);
                }
                return;
            }
        }
    }
}
