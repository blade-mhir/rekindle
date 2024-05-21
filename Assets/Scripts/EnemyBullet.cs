using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Speed of the bullet
    [SerializeField] private float damage = 1f; // Damage dealt by the bullet

    public float Speed { get { return speed; } }

    [SerializeField] private float minX = -6.52f; // Min X boundary for bullet (customizable in inspector)
    [SerializeField] private float maxX = 7.20f;  // Max X boundary for bullet (customizable in inspector)
    [SerializeField] private float minY = -7.5f;  // Min Y boundary for bullet (customizable in inspector)
    [SerializeField] private float maxY = 6.17f;  // Max Y boundary for bullet (customizable in inspector)

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

    public void SetDamage(float damageValue)
    {
        damage = damageValue;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name); // Debug log to see which object is colliding

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player detected!"); // Debug log to confirm player collision
            // Deal damage to the player and destroy the bullet
            HealthController healthController = collision.gameObject.GetComponent<HealthController>();
            if (healthController != null)
            {
                Debug.Log("HealthController found, applying damage: " + damage);
                healthController.TakeDamage(damage);
            }
            else
            {
                Debug.LogError("HealthController component not found on Player!");
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Obstacle detected!"); // Debug log to confirm obstacle collision
            // Destroy the bullet if it hits an obstacle
            Destroy(gameObject);
        }
    }
}
