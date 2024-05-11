using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int damage = 1; // Damage value
    private int direction = 1; // Default direction (right)

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
        if (transform.position.x < -6.25f || transform.position.x > 6.92f)
        {
            Destroy(gameObject);
        }

        // Check if bullet goes beyond Y position and destroy
        if (transform.position.y < -6.95f || transform.position.y > 5.93f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // if (collision.gameObject.CompareTag("Enemy"))
        // {
        //     // Attempt to get the EnemyController component
        //     EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();

        //     if (enemyController != null)
        //     {
        //         // Decrease enemy health based on the bullet's damage value
        //         enemyController.TakeDamage(damage);

        //         // Destroy the bullet
        //         Destroy(gameObject);
        //     }
        // }
    }
}
