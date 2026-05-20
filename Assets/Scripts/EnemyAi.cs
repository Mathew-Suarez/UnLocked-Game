using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum EnemyState { Roaming, Chasing }
    [SerializeField] private EnemyState currentState = EnemyState.Roaming;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    [Header("Detection Settings")]
    public float aggroRange = 5f;
    public LayerMask playerLayer;
    private Transform playerTransform;

    [Header("Roaming Settings")]
    public float roamRadius = 4f;       // How far the enemy can walk from its start position
    public float minWaitTime = 1f;      // Minimum time spent waiting at a spot
    public float maxWaitTime = 3f;      // Maximum time spent waiting at a spot

    private Vector2 startPosition;
    private Vector2 roamTarget;
    private float waitTimer;
    private bool isWaiting = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;

        // Pick the very first roaming destination
        PickNewRoamTarget();
    }

    private void Update()
    {
        // 1. Always look for the player first
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, aggroRange, playerLayer);

        if (playerCollider != null)
        {
            playerTransform = playerCollider.transform;
            currentState = EnemyState.Chasing;
        }
        else
        {
            currentState = EnemyState.Roaming;
        }

        // 2. Handle behaviors based on the current state
        switch (currentState)
        {
            case EnemyState.Roaming:
                HandleRoaming();
                break;
            case EnemyState.Chasing:
                HandleChasing();
                break;
        }
    }

    private void FixedUpdate()
    {
        // Move the Rigidbody using fixed velocity calculation
        if (currentState == EnemyState.Chasing || (currentState == EnemyState.Roaming && !isWaiting))
        {
            rb.velocity = moveDirection * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void HandleRoaming()
    {
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                PickNewRoamTarget();
            }
            return;
        }

        // Calculate direction to the target position
        moveDirection = (roamTarget - (Vector2)transform.position).normalized;

        // Check if the enemy has reached the target position
        if (Vector2.Distance(transform.position, roamTarget) < 0.2f)
        {
            isWaiting = true;
            moveDirection = Vector2.zero;
            waitTimer = Random.Range(minWaitTime, maxWaitTime);
        }
    }

    private void HandleChasing()
    {
        isWaiting = false; // Cancel waiting if player is spotted

        if (playerTransform != null)
        {
            moveDirection = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
        }
    }

    private void PickNewRoamTarget()
    {
        // Choose a random point inside a circle around the enemy's original spawn position
        Vector2 randomCirclePoint = Random.insideUnitCircle * roamRadius;
        roamTarget = startPosition + randomCirclePoint;
    }

    // Visualizes the detection range and roaming boundaries in the Scene View
    private void OnDrawGizmosSelected()
    {
        // Red circle shows player detection range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);

        // Blue circle shows the boundaries of where the enemy can wander
        Gizmos.color = Color.blue;
        if (Application.isPlaying)
            Gizmos.DrawWireSphere(startPosition, roamRadius);
        else
            Gizmos.DrawWireSphere(transform.position, roamRadius);
    }
}