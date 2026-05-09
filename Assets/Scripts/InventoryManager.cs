using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;    // Required for the Image component
using TMPro;            // Required for TextMesh Pro

public class InventoryManager : MonoBehaviour
{
    // This allows the ItemPickUp script to find this Manager instantly
    public static InventoryManager instance;


    [Header("UI Slot References")]
    public Image itemIconInSlot;            // Drag your 'ItemIcon' UI object here
    public TextMeshProUGUI quantityText;    // Drag your 'QuantityText' (TMP) here

    [Header("Inventory Data")]
    public int currentQuantity = 0;         // Tracks how many items you have

    private void Awake()
    {
        // Setup the Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // At the start of the game, make sure the UI is hidden/empty
        if (currentQuantity == 0)
        {
            itemIconInSlot.color = new Color(1, 1, 1, 0); // Transparent
            quantityText.text = "";                        // Empty text
        }
    }

    // This function is called by the ItemPickUp script
    public void UpdateSlot(Sprite newSprite)
    {
        // 1. Increment the counter
        currentQuantity++;
        // 2. Update the Image
        itemIconInSlot.sprite = newSprite;
        itemIconInSlot.color = Color.white; // Make the icon visible (Alpha = 1)

        // 3. Update the TextMesh Pro text
        // We convert the integer 'currentQuantity' to a string so the UI can read it
        quantityText.text = currentQuantity.ToString();

        // Ensure the text color is visible (White)
        quantityText.color = Color.white;

        Debug.Log("Inventory Updated! Current Items: " + currentQuantity);
    }
}