using UnityEngine;

public class OrbShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Bullet prefab to be assigned in the inspector
    [SerializeField] private float fireRate = 1f; // Fire rate in seconds
    [SerializeField] private float bulletDamage = 1f; // Damage each bullet deals

    private Transform playerTransform;
    private float timeSinceLastShot = 0f; // Time since the last shot was fired

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Debug statement to check if playerTransform is assigned
        if (playerTransform == null)
        {
            Debug.LogError("Player transform not found!");
        }
    }

    private void Update()
    {
        // If enough time has passed since the last shot, shoot at the player
        if (Time.time - timeSinceLastShot >= 1f / fireRate)
        {
            Shoot();
            timeSinceLastShot = Time.time;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab != null)
        {
            // Define spread angle for the cone (adjust as needed)
            float spreadAngle = 360f; // Full circle

            // Calculate rotation step for each bullet (ensure it divides spread angle for equal spacing)
            float rotationStep = spreadAngle / 8f;

            // Loop to shoot 8 bullets with specific rotations
            for (int i = 0; i < 8; i++)
            {
                // Calculate rotation based on the loop iteration
                float currentAngle = i * rotationStep;

                // Rotate the base direction
                Vector3 bulletDirection = Quaternion.Euler(0f, 0f, currentAngle) * Vector3.right;

                // Instantiate and apply force with consistent speed
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

                // Set the bullet's damage
                bullet.GetComponent<EnemyBullet>().SetDamage(bulletDamage);

                // Apply velocity to the bullet
                bulletRb.velocity = bulletDirection * bullet.GetComponent<EnemyBullet>().Speed;
            }
        }
    }
}
