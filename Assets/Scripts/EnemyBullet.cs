using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Speed of the bullet
    [SerializeField] private float damage = 1f; // Damage dealt by the bullet

    public float Speed { get { return speed; } }

        void Update()
    {
        // Move the bullet based on its velocity
        transform.Translate(GetComponent<Rigidbody2D>().velocity * Time.deltaTime);

        // Check if bullet goes beyond X position and destroy
        if (transform.position.x < -6.52f || transform.position.x > 7.20f)
        {
            Destroy(gameObject);
        }

        // Check if bullet goes beyond Y position and destroy
        if (transform.position.y < -7.5f || transform.position.y > 6.17f)
        {
            Destroy(gameObject);
        }
    }


    public void SetDamage(float damageValue)
    {
        damage = damageValue;
    }

   private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.name); // Debug log to see which object is colliding

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected!"); // Debug log to confirm player collision
            // Deal damage to the player and destroy the bullet
            other.GetComponent<HealthController>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Obstacle detected!"); // Debug log to confirm obstacle collision
            // Destroy the bullet if it hits an obstacle
            Destroy(gameObject);
        }
    }

}
