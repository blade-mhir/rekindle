using System.Collections;
using UnityEngine;

public class LaserCardActivation : MonoBehaviour
{
    [SerializeField] private GameObject laserBulletPrefab; // Reference to the laser bullet prefab
    [SerializeField] private float laserDuration = 10f; // Duration of laser powerup (customizable in inspector)
    [SerializeField] private float cooldownDuration = 30f; // Cooldown duration for laser powerup (customizable in inspector)
    [SerializeField] private GameObject laserPowerUpObject;

    private bool isLaserActive = false; // Flag for laser powerup state
    private float laserStartTime;
    private bool isCooldownActive = false;
    private float cooldownStartTime;

    private Shooting shootingScript; // Reference to the Shooting script

    private void Start()
    {
        shootingScript = GetComponent<Shooting>(); // Get the Shooting script attached to the player
    }

    private void Update()
    {
        // Activate laser powerup with "E" key and check cooldown
        if (Input.GetKeyDown(KeyCode.E) && !isCooldownActive)
        {
            ActivateLaserPowerUp();
        }

        // Check for laser powerup duration ending
        if (isLaserActive && Time.time >= laserStartTime + laserDuration)
        {
            DeactivateLaserPowerUp();
        }

        // Check for cooldown ending
        if (isCooldownActive && Time.time >= cooldownStartTime + cooldownDuration)
        {
            isCooldownActive = false;
        }
    }

    private void ActivateLaserPowerUp()
    {
        isLaserActive = true;
        laserStartTime = Time.time;

        shootingScript.SetLaserBulletPrefab(laserBulletPrefab);

        // Enable the laser power-up object
        if (laserPowerUpObject != null)
        {
            laserPowerUpObject.SetActive(true);
        }

        // Start cooldown
        isCooldownActive = true;
        cooldownStartTime = Time.time;
    }

    private void DeactivateLaserPowerUp()
    {
        isLaserActive = false;

        shootingScript.ResetBulletPrefab();

        // Disable the laser power-up object
        if (laserPowerUpObject != null)
        {
            laserPowerUpObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!CardManager.instance.IsDashCardActivated() && !CardManager.instance.IsInviCardActivated() && collision.gameObject.CompareTag("LaserCard"))
        {
            CardManager.instance.ActivateLaserCard();
            ActivateLaserPowerUp();
            Destroy(collision.gameObject); // Destroy the power-up on collision
        }
    }
}
