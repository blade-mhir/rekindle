using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int damage = 1; // Damage value
    private int direction = 1; // Default direction (right)

    [SerializeField] private float minX = -6.52f; // Min X boundary for bullet (customizable in inspector)
    [SerializeField] private float maxX = 7.20f;  // Max X boundary for bullet (customizable in inspector)
    [SerializeField] private float minY = -7.5f;  // Min Y boundary for bullet (customizable in inspector)
    [SerializeField] private float maxY = 6.17f;  // Max Y boundary for bullet (customizable in inspector)


    // Method to set the bullet direction
    public void SetDirection(int newDirection)
    {
        direction = newDirection;
        // Flip the bullet sprite based on the direction
        SpriteRenderer bulletRenderer = GetComponent<SpriteRenderer>();
        bulletRenderer.flipX = direction < 0;
    }

    void Update()
    {
        // Move the bullet based on its velocity
        transform.Translate(GetComponent<Rigidbody2D>().velocity * Time.deltaTime);

        // Check if bullet goes beyond X position and destroy
        if (transform.position.x < minX || transform.position.x > maxX)
        {
            Destroy(gameObject);
        }

        // Check if bullet goes beyond Y position and destroy
        if (transform.position.y < minY || transform.position.y > maxY)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collision is with an enemy
        if (collision.gameObject.GetComponent<EnemyHealth>() != null)
        {
            // Get the enemy health script
            var enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            // Deal damage to the enemy
            enemyHealth.TakeDamage(damage);
            // Destroy the bullet after hitting the enemy
            Destroy(gameObject);
        }
    }
}

public class LaserBulletController : BulletController
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if collision is with an enemy
        if (collision.gameObject.GetComponent<EnemyHealth>() != null)
        {
            // Get the enemy health script
            var enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            // Deal damage to the enemy
            enemyHealth.TakeDamage(damage);
        }
    }
}
