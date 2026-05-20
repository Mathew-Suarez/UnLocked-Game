using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The constant speed the enemy moves at all times.")]
    public float walkSpeed = 1.5f;

    private int facingDirection = -1;   // Assumes your sprite naturally faces Left (-1)
    private Rigidbody2D rb;
    private Transform player;
    private bool isChasing;

    [Header("Roaming Settings")]
    public float roamRadius = 4f;
    public float minWaitTime = 1.5f;
    public float maxWaitTime = 3.5f;

    private Vector2 startPosition;
    private Vector2 roamTarget;
    private float waitTimer;
    private bool isWaiting = false;
    private Vector2 lastChaseDirection; // Stores the direction the enemy was heading right before losing you

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;

        // Start by picking a relaxed, nearby roaming target
        PickNewRoamTarget();
    }

    void Update()
    {
        // Handle flipping automatically based on horizontal movement speed
        if (rb.velocity.x > 0.05f && facingDirection == -1)
        {
            Flip(); // Move Right, Face Right
        }
        else if (rb.velocity.x < -0.05f && facingDirection == 1)
        {
            Flip(); // Move Left, Face Left
        }
    }

    private void FixedUpdate()
    {
        if (isChasing)
        {
            HandleChasingMovement();
        }
        else
        {
            HandleRoamingMovement();
        }
    }

    private void HandleChasingMovement()
    {
        isWaiting = false;

        if (player != null)
        {
            // Calculate direction strictly as a clean, uniform vector
            Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;

            // Save this direction in case the player breaks line-of-sight
            lastChaseDirection = direction;

            rb.velocity = direction * walkSpeed;
        }
    }

    private void HandleRoamingMovement()
    {
        if (isWaiting)
        {
            rb.velocity = Vector2.zero;
            waitTimer -= Time.fixedDeltaTime;

            if (waitTimer <= 0f)
            {
                isWaiting = false;
                PickNewRoamTarget();
            }
            return;
        }

        // Calculate a clean normalized vector pointing directly to the target point
        Vector2 direction = (roamTarget - (Vector2)transform.position).normalized;
        rb.velocity = direction * walkSpeed;

        // Check if destination is reached
        if (Vector2.Distance(transform.position, roamTarget) < 0.2f)
        {
            isWaiting = true;
            waitTimer = Random.Range(minWaitTime, maxWaitTime);
        }
    }

    private void PickNewRoamTarget()
    {
        Vector2 randomCirclePoint = Random.insideUnitCircle * roamRadius;
        roamTarget = startPosition + randomCirclePoint;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player == null)
            {
                player = collision.transform;
            }
            isChasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isChasing = false;

            // 1. Keep coasting forward in the same direction it was chasing you at normal walk speed
            rb.velocity = lastChaseDirection * walkSpeed;

            // 2. Set their new anchor point right here so they roam this specific area
            startPosition = transform.position;

            // 3. Project the next target point directly ahead along their path 
            roamTarget = startPosition + (lastChaseDirection * (roamRadius * 0.75f));
            isWaiting = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        if (Application.isPlaying)
            Gizmos.DrawWireSphere(startPosition, roamRadius);
        else
            Gizmos.DrawWireSphere(transform.position, roamRadius);
    }
}