using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
  [SerializeField] private float moveSpeed = 2f; // Configure movement speed in the inspector
  [SerializeField] private float attackRange = 1f;
  [SerializeField] private int damageAmount = 1; // Configure enemy damage in the inspector

  private Transform playerTransform;
  private Rigidbody2D rb;

  private void Awake()
  {
    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    rb = GetComponent<Rigidbody2D>();
  }

  private void FixedUpdate()
  {
    float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
    if (distanceToPlayer <= attackRange)
    {
      // Move towards player
      Vector2 direction = playerTransform.position - transform.position;
      direction.Normalize();
      rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);

      // Deal damage to player on collision
      OnCollisionEnter2D(null); // Simulate collision for damage
    }
  }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    // Check if collided with player
    if (collision != null && collision.gameObject.CompareTag("Player"))
    {
        var playerHealth = playerTransform.GetComponent<HealthController>();
        if (playerHealth != null)
        {
        playerHealth.TakeDamage(damageAmount);
        }
    }
    }

}