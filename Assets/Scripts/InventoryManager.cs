using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [Header("UI Slot References")]
    public Image itemIconInSlot;
    public TextMeshProUGUI quantityText;

    [Header("Inventory Data")]
    public int currentQuantity = 0;
    private string currentItemName = "";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // NO DontDestroyOnLoad here - the parent handles persistence
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (currentQuantity == 0)
        {
            itemIconInSlot.color = new Color(1, 1, 1, 0);
            quantityText.text = "";
        }
    }

    public void UpdateSlot(Sprite newSprite)
    {
        currentQuantity++;
        itemIconInSlot.sprite = newSprite;
        itemIconInSlot.color = Color.white;
        quantityText.text = currentQuantity.ToString();
        quantityText.color = Color.white;

        currentItemName = newSprite.name;
        Debug.Log("Inventory Updated! Current Items: " + currentQuantity);
    }

    public InventoryItemData GetSaveData()
    {
        if (currentQuantity > 0 && !string.IsNullOrEmpty(currentItemName))
        {
            return new InventoryItemData { itemName = currentItemName, quantity = currentQuantity };
        }
        return null;
    }

    public void LoadFromSaveData(InventoryItemData data)
    {
        ClearInventory();
        if (data != null && data.quantity > 0)
        {
            currentQuantity = data.quantity;
            currentItemName = data.itemName;

            Sprite loadedSprite = Resources.Load<Sprite>("Sprites/" + data.itemName);
            if (loadedSprite != null)
            {
                itemIconInSlot.sprite = loadedSprite;
                itemIconInSlot.color = Color.white;
            }
            else
            {
                Debug.LogWarning("Sprite not found in Resources/Sprites: " + data.itemName);
            }
            quantityText.text = currentQuantity.ToString();
            quantityText.color = Color.white;
        }
    }

    public void ClearInventory()
    {
        currentQuantity = 0;
        currentItemName = "";
        itemIconInSlot.sprite = null;
        itemIconInSlot.color = new Color(1, 1, 1, 0);
        quantityText.text = "";
    }
}