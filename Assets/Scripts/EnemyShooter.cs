using UnityEngine;

public class EnemyShooter : MonoBehaviour
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
        if (bulletPrefab != null && playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<EnemyBullet>().SetDamage(bulletDamage); // Updated to EnemyBullet
            bullet.GetComponent<Rigidbody2D>().velocity = direction * bullet.GetComponent<EnemyBullet>().Speed; // Updated to EnemyBullet
        }
    }
}
