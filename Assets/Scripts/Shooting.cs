using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Reference to the bullet prefab
    [SerializeField] private Transform firePoint; // Reference to the fire point transform
    [SerializeField] private float bulletForce = 10f; // Bullet force (customizable in inspector)
    [SerializeField] private float fireRate = 0.5f; // Fire rate (customizable in inspector)
    [SerializeField] private float shotgunDuration = 5f; // Duration of shotgun powerup (customizable in inspector)

    private float nextFireTime = 0f;
    private bool isShotgunActive = false;
    private float shotgunStartTime;

    private void Update()
    {
        if (isShotgunActive && Time.time >= nextFireTime)
        {
            ShootShotgun();
            nextFireTime = Time.time + 1f / fireRate;
        }
        else if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }

        // Check for shotgun powerup duration ending
        if (isShotgunActive && Time.time >= shotgunStartTime + shotgunDuration)
        {
            isShotgunActive = false;
        }
    }

    private void Shoot()
    {
    // Get mouse position in world space
    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    // Calculate firing direction
    Vector3 firingDirection = mousePos - firePoint.position;
    firingDirection.Normalize();

    // Get the current movement direction (optional)
    Vector3 movementDirection = transform.right; // Replace with your movement calculation

    // Combine firing direction and movement (optional)
    // Vector3 combinedDirection = firingDirection + movementDirection; // Uncomment for combined movement

    // Instantiate and apply force with consistent speed
    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
    if (bulletRb != null)
    {
        // Adjust bulletForce to achieve desired speed
        bulletRb.AddForce(firingDirection.normalized * bulletForce, ForceMode2D.Impulse);
    }
    }


    private void ShootShotgun()
    {
        // Calculate base firing direction based on mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 firingDirection = mousePos - firePoint.position;
        firingDirection.Normalize();

        // Define spread angle for the cone (adjust as needed)
        float spreadAngle = 20f; // Adjust this value to control the cone spread

        // Calculate rotation step for each bullet (ensure it divides spread angle for equal spacing)
        float rotationStep = spreadAngle / 2f;

        // Loop to shoot 3 bullets with specific rotations
        for (int i = 0; i < 3; i++)
        {
            // Calculate rotation based on the loop iteration
            float currentAngle = i * rotationStep;

            // Rotate the base direction
            Vector3 bulletDirection = Quaternion.Euler(0f, 0f, currentAngle) * firingDirection;

            // Get the current movement direction (optional)
            Vector3 movementDirection = transform.right; // Replace with your movement calculation

            // Combine firing direction and movement (optional)
            // Vector3 combinedDirection = firingDirection + movementDirection; // Uncomment for combined movement

            // Instantiate and apply force with consistent speed
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
            // Adjust bulletForce to achieve desired speed
            bulletRb.AddForce(bulletDirection.normalized * bulletForce, ForceMode2D.Impulse);
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Shotgun"))
        {
            isShotgunActive = true;
            shotgunStartTime = Time.time;
            Destroy(collision.gameObject); // Destroy the powerup on collision
        }
    }
}
