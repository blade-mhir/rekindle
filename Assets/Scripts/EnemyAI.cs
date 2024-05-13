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
    InvokeRepeating("DealDamage", 0f, 1f); // Adjust delay and interval (seconds)
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

    private void DealDamage() {
  // Check if player is still in range before dealing damage
  if (Vector2.Distance(transform.position, playerTransform.position) <= attackRange) {
    var playerHealth = playerTransform.GetComponent<HealthController>();
    if (playerHealth != null) {
      playerHealth.TakeDamage(damageAmount);
    }
  }
}


}
