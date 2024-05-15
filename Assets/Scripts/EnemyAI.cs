using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f; // Configure movement speed in the inspector
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float damagePerInterval = 1f; // Configure damage per interval in the inspector
    [SerializeField] private float damageInterval = 1f; // Configure damage interval in seconds (e.g., 1 for every second)

    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isPlayerColliding = false; // Flag to track player collision
    private float timeSinceLastDamage = 0f; // Tracks time since last damage application

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // Move towards player only if within attack range
        if (distanceToPlayer <= attackRange)
        {
            Vector2 direction = playerTransform.position - transform.position;
            direction.Normalize();
            rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
        }

        // Reset flag if out of range
        if (distanceToPlayer > attackRange)
        {
            isPlayerColliding = false;
            timeSinceLastDamage = 0f; // Reset timer when out of range
        }

        // Deal damage at set interval while colliding
        if (isPlayerColliding)
        {
            timeSinceLastDamage += Time.deltaTime;
            if (timeSinceLastDamage >= damageInterval)
            {
                var playerHealth = playerTransform.GetComponent<HealthController>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damagePerInterval);
                }
                timeSinceLastDamage = 0f; // Reset timer after damage application
            }
        }
    }

    // Implement OnCollisionEnter2D to set the flag when colliding with the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Player"))
        {
            isPlayerColliding = true;
        }
    }

    // Implement OnCollisionExit2D to reset the flag and timer when no longer colliding
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Player"))
        {
            isPlayerColliding = false;
            timeSinceLastDamage = 0f; // Reset timer on exit
        }
    }
}
