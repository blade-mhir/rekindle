using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Reference to the bullet prefab
    [SerializeField] private Transform firePoint; // Reference to the fire point transform
    [SerializeField] private float bulletForce = 10f; // Bullet force (customizable in inspector)
    [SerializeField] private float fireRate = 0.5f; // Fire rate (customizable in inspector)
    [SerializeField] private float shotgunDuration = 5f; // Duration of shotgun powerup (customizable in inspector)
    [SerializeField] private float eightPowerUpDuration = 5f; // Duration of eight powerup (customizable in inspector)

    private float nextFireTime = 0f;
     private bool isPowerUpActive = false; // Flag for any active powerup
    private string activePowerUp; // Name of the currently active powerup
    private float powerUpStartTime;

    private float baseFireRate; // Store the default fire rate
    private float baseBulletForce; // Store the default bullet force


    void Start()
    {
        baseFireRate = fireRate; // Store the default fire rate
        baseBulletForce = bulletForce; // Store the default bullet force
    }
        
    private void Update()
    {
        if (isPowerUpActive && Time.time >= nextFireTime)
        {
            switch (activePowerUp)
            {
                case "Shotgun":
                    ShootShotgun();
                    break;
                case "Eight":
                    ShootEight();
                    break;
                default:
                    Debug.LogError("Unexpected powerup active: " + activePowerUp);
                    break;
            }
            nextFireTime = Time.time + 1f / fireRate;
        }
        else if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }

        // Check for powerup duration ending
        if (isPowerUpActive && Time.time >= powerUpStartTime + GetPowerUpDuration(activePowerUp))
        {
            DeactivatePowerUp();
        }
    }

    private void Shoot()
    {
   // Calculate firing direction relative to the fire point
   Vector3 mousePos = Input.mousePosition;
   Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -Camera.main.transform.position.z));
   Vector3 firingDirection = mouseWorldPos - firePoint.position;
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
        // Calculate firing direction relative to the fire point
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -Camera.main.transform.position.z));
        Vector3 firingDirection = mouseWorldPos - firePoint.position;
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

     private void ShootEight()
    {
        // Calculate firing direction relative to the fire point
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -Camera.main.transform.position.z));
        Vector3 firingDirection = mouseWorldPos - firePoint.position;
        firingDirection.Normalize();

        // Define spread angle for the cone (adjust as needed)
        float spreadAngle = 360f; // Adjust for cone shape if desired

        // Calculate rotation step for each bullet (ensure it divides spread angle for equal spacing)
        float rotationStep = spreadAngle / 8f;

        // Loop to shoot 8 bullets with specific rotations
        for (int i = 0; i < 8; i++)
        {
            // Calculate rotation based on the loop iteration
            float currentAngle = i * rotationStep;

            // Rotate the base direction
            Vector3 bulletDirection = Quaternion.Euler(0f, 0f, currentAngle) * firingDirection;

            // Get the current movement direction (optional)
            // Vector3 movementDirection = transform.right; // Replace with your movement calculation

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
            ActivatePowerUp("Shotgun");
            Destroy(collision.gameObject); // Destroy the powerup on collision
        }
        else if (collision.gameObject.CompareTag("Eight"))
        {
            ActivatePowerUp("Eight");
            Destroy(collision.gameObject); // Destroy the powerup on collision
        }
    }

    private void ActivatePowerUp(string powerUpName)
    {
        if (isPowerUpActive && powerUpName != activePowerUp)
        {
            DeactivatePowerUp(); // Deactivate previous powerup if different
        }

        isPowerUpActive = true;
        activePowerUp = powerUpName;
        powerUpStartTime = Time.time;
    }

    private void DeactivatePowerUp()
    {
        isPowerUpActive = false;
        activePowerUp = "";
    }

    private float GetPowerUpDuration(string powerUpName)
    {
        switch (powerUpName)
        {
            case "Shotgun":
                return shotgunDuration;
            case "Eight":
                return eightPowerUpDuration;
            default:
                Debug.LogError("Unexpected powerup name: " + powerUpName);
                return 0f;
        }
    }

    public void SetFirePowerUpValues(float increasedFireRate, float increasedBulletForce)
    {
        // Set modified values during "Fire" power-up
        fireRate = increasedFireRate;
        bulletForce = increasedBulletForce;
    }


    public void ResetFirePowerUpValues()
    {
        // Reset to default values when "Fire" power-up ends
        fireRate = baseFireRate;
        bulletForce = baseBulletForce;
    }
}
