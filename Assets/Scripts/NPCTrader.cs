using UnityEngine;
using UnityEngine.UI; // Required for the Button components
using System.Collections; // Required for Coroutines

public class NPCTrader : MonoBehaviour, IInteractable
{
    [Header("UI Shop Reference")]
    [Tooltip("Drag your ShopPanel GameObject here.")]
    [SerializeField] private GameObject shopPanel;

    [Header("Alert UI Settings")]
    [Tooltip("Drag a TextMeshProUGUI element here that will display the error message.")]
    [SerializeField] private TMPro.TextMeshProUGUI alertTextElement;
    [SerializeField] private float alertDuration = 2f;

    [Header("Button References (DRAG BUTTONS HERE TO AUTO-CONNECT)")]
    [SerializeField] private Button buttonA;
    [SerializeField] private Button buttonB;
    [SerializeField] private Button buttonC;
    [SerializeField] private Button closeButton;

    [Header("Scrap Core Settings")]
    [Tooltip("Type the exact file name of your scrap sprite asset (e.g., scraps)")]
    [SerializeField] private string scrapSpriteName = "scraps";

    [Header("Available Items for Trade")]
    [SerializeField] private Sprite itemA_Sprite;
    [SerializeField] private int itemA_Cost = 3;

    [Space]
    [SerializeField] private Sprite itemB_Sprite;
    [SerializeField] private int itemB_Cost = 6;

    [Space]
    [SerializeField] private Sprite itemC_Sprite;
    [SerializeField] private int itemC_Cost = 4;

    private Coroutine alertCoroutine;

    private void Start()
    {
        if (alertTextElement != null) alertTextElement.text = "";

        if (buttonA != null) buttonA.onClick.AddListener(BuyItemA);
        if (buttonB != null) buttonB.onClick.AddListener(BuyItemB);
        if (buttonC != null) buttonC.onClick.AddListener(BuyItemC);
        if (closeButton != null) closeButton.onClick.AddListener(CloseShop);
    }

    public void Interact(Transform player)
    {
        if (shopPanel != null) shopPanel.SetActive(true);
    }

    public void BuyItemA() { TryProcessPurchase(itemA_Sprite, itemA_Cost); }
    public void BuyItemB() { TryProcessPurchase(itemB_Sprite, itemB_Cost); }
    public void BuyItemC() { TryProcessPurchase(itemC_Sprite, itemC_Cost); }

    public void CloseShop()
    {
        if (shopPanel != null) shopPanel.SetActive(false);
    }

    private void TryProcessPurchase(Sprite rewardSprite, int cost)
    {
        if (InventoryManager.instance == null) return;

        // 1. Scan your 4 database blocks for your scrap name tag string
        int currentScraps = InventoryManager.instance.GetItemQuantity(scrapSpriteName);
        Debug.Log($"[SHOP ENGINE] Balance found: {currentScraps}. Item Price tag: {cost}");

        if (currentScraps >= cost)
        {
            // 2. Safely deduct only the cost value from your scrap count
            InventoryManager.instance.RemoveItems(scrapSpriteName, cost);
            Debug.Log($"[SHOP ENGINE] Successfully deducted {cost} scraps. Balance is now: {currentScraps - cost}");

            // 3. Deliver your bought item to the next vacant storage slot safely
            if (rewardSprite != null)
            {
                InventoryManager.instance.UpdateSlot(rewardSprite);
            }

            CloseShop();
        }
        else
        {
            ShowAlert("Insufficient Scraps!");
        }
    }

    private void ShowAlert(string message)
    {
        if (alertTextElement != null)
        {
            if (alertCoroutine != null) StopCoroutine(alertCoroutine);
            alertCoroutine = StartCoroutine(FlashAlertMessage(message));
        }
    }

    IEnumerator FlashAlertMessage(string message)
    {
        alertTextElement.text = message;
        alertTextElement.color = Color.red;
        yield return new WaitForSeconds(alertDuration);
        alertTextElement.text = "";
    }
}
