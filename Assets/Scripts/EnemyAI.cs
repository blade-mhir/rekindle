using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f; // Configure movement speed in the inspector
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float damagePerInterval = 1f; // Configure damage per interval in the inspector
    [SerializeField] private float damageInterval = 1f; // Configure damage interval in seconds (e.g., 1 for every second)
    
    // Define static boundaries for all enemies
    public static float minX = -6.25f; // Minimum x coordinate
    public static float maxX = 6.92f; // Maximum x coordinate
    public static float minY = -7.11f; // Minimum y coordinate
    public static float maxY = 5.93f; // Maximum y coordinate

    private Transform playerTransform;
    private Rigidbody2D rb;
    private bool isPlayerColliding = false; // Flag to track player collision
    private float timeSinceLastDamage = 0f; // Tracks time since last damage application

    private PlayerController playerController;
    private Vector2 randomDirection;
    private float changeDirectionTime = 2f;
    private float nextChangeDirectionTime = 0f;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        playerController = playerTransform.GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (playerController != null && playerController.IsInvisible())
        {
            RandomWander();
        }
        else
        {
            TrackAndAttackPlayer();
        }
    }

    private void TrackAndAttackPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackRange)
        {
            Vector2 direction = playerTransform.position - transform.position;
            direction.Normalize();
            Vector2 newPos = rb.position + direction * moveSpeed * Time.deltaTime;
            rb.MovePosition(ClampPosition(newPos));
        }

        if (distanceToPlayer > attackRange)
        {
            isPlayerColliding = false;
            timeSinceLastDamage = 0f; // Reset timer when out of range
        }

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

    private void RandomWander()
    {
        if (Time.time >= nextChangeDirectionTime)
        {
            randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            nextChangeDirectionTime = Time.time + changeDirectionTime;
        }

        Vector2 newPos = rb.position + randomDirection * moveSpeed * Time.deltaTime;
        rb.MovePosition(ClampPosition(newPos));
    }

    private Vector2 ClampPosition(Vector2 position)
    {
        float clampedX = Mathf.Clamp(position.x, minX, maxX);
        float clampedY = Mathf.Clamp(position.y, minY, maxY);
        return new Vector2(clampedX, clampedY);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Player"))
        {
            isPlayerColliding = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Player"))
        {
            isPlayerColliding = false;
            timeSinceLastDamage = 0f; // Reset timer on exit
        }
    }
}
