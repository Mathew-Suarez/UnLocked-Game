using UnityEngine;

[System.Serializable]
public class EquippableItem
{
    public string itemName;          // Set this to "Sword" in the Inspector
    public GameObject itemGameObject; // Link the child 'Sword_Weapon' here
}

public class PlayerEquipment : MonoBehaviour
{
    [Header("Equipment Database")]
    [Tooltip("Add your items here and link them to their player game objects.")]
    public EquippableItem[] equippableItems;

    [Header("Combat Settings")]
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask enemyLayer; // Set this to your Enemy layer

    private string currentlyEquippedItem = "";
    private float nextAttackTime = 0f;

    void Update()
    {
        // 1. Trigger attack when pressing Left Mouse Button (or a mobile UI attack button)
        if (currentlyEquippedItem == "Sword" && Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            AttackCycle();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    public void EquipItem(string itemNameToEquip)
    {
        Debug.Log($"Equipment system checking for: {itemNameToEquip}");
        bool itemFound = false;

        // 2. Loop through your database to find the item
        for (int i = 0; i < equippableItems.Length; i++)
        {
            if (string.Equals(equippableItems[i].itemName, itemNameToEquip, System.StringComparison.OrdinalIgnoreCase))
            {
                if (equippableItems[i].itemGameObject != null)
                {
                    equippableItems[i].itemGameObject.SetActive(true); // Turn graphics ON
                }
                currentlyEquippedItem = equippableItems[i].itemName;
                itemFound = true;
                Debug.Log($"Successfully equipped: {currentlyEquippedItem}");
            }
            else
            {
                // Automatically turn OFF all other unselected weapons
                if (equippableItems[i].itemGameObject != null)
                {
                    equippableItems[i].itemGameObject.SetActive(false);
                }
            }
        }

        if (!itemFound)
        {
            Debug.LogWarning($"Item '{itemNameToEquip}' is not configured in the PlayerEquipment database.");
        }
    }

    private void AttackCycle()
    {
        Debug.Log("Swinging Sword!");

        // FIXED: Added 'All' to Physics2D.OverlapCircle
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit enemy: " + enemy.name);

            // Example Hook for later: 
            // enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
        }
    }


    private void OnDrawGizmosSelected()
    {
        // Draws a yellow circle in your Scene view so you can see your attack range boundaries
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
