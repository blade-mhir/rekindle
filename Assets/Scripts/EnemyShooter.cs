using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Bullet prefab to be assigned in the inspector
    [SerializeField] private float fireRate = 1f; // Fire rate in seconds
    [SerializeField] private float bulletDamage = 1f; // Damage each bullet deals
    [SerializeField] private float initialDelay = 0f; // Initial delay before the enemy starts shooting

    private Transform playerTransform;
    private float timeSinceLastShot = 0f; // Time since the last shot was fired
    private bool canShoot = false; // Flag to control shooting after the delay

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Debug statement to check if playerTransform is assigned
        if (playerTransform == null)
        {
            Debug.LogError("Player transform not found!");
        }
    }

    private void Start()
    {
        StartCoroutine(StartShootingAfterDelay());
    }

    private void Update()
    {
        // If enough time has passed since the last shot and shooting is allowed, shoot at the player
        if (canShoot && Time.time - timeSinceLastShot >= 1f / fireRate)
        {
            Shoot();
            timeSinceLastShot = Time.time;
        }
    }

    private IEnumerator StartShootingAfterDelay()
    {
        // Wait for the specified initial delay
        yield return new WaitForSeconds(initialDelay);
        canShoot = true; // Allow shooting after the delay
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
