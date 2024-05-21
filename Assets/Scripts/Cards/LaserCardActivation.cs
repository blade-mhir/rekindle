using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LaserCardActivation : MonoBehaviour
{
    [SerializeField] private GameObject laserBulletPrefab; // Reference to the laser bullet prefab
    [SerializeField] private float laserDuration = 10f; // Duration of laser powerup (customizable in inspector)
    [SerializeField] private float cooldownDuration = 15f; // Cooldown duration after laser powerup ends
    [SerializeField] private GameObject laserPowerUpObject; // Reference to the laser power-up object
    [SerializeField] private Image cooldownFillImage; // Reference to the cooldown fill image


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
        // Activate laser powerup with "E" key
        if (Input.GetKeyDown(KeyCode.E) && !isCooldownActive && CardManager.instance.IsDashCardActivated())
        {
            ActivateLaserPowerUp();
        }

        // Check for laser powerup duration ending
        if (isLaserActive && Time.time >= laserStartTime + laserDuration)
        {
            DeactivateLaserPowerUp();
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

        // Start cooldown coroutine
        StartCoroutine(CooldownCoroutine());
    }

    private void DeactivateLaserPowerUp()
    {
        isLaserActive = false;

        shootingScript.ResetBulletPrefab();
    }

    private IEnumerator CooldownCoroutine()
    {
        isCooldownActive = true;
        cooldownStartTime = Time.time;

        while (Time.time < cooldownStartTime + cooldownDuration)
        {
            float timeSinceCooldownStart = Time.time - cooldownStartTime;
            float cooldownFillAmount = Mathf.Clamp01(timeSinceCooldownStart / cooldownDuration);
            cooldownFillImage.fillAmount = 1f - cooldownFillAmount;

            yield return null; // Wait for the next frame
        }

        isCooldownActive = false;
        cooldownFillImage.fillAmount = 0f; // Reset cooldown fill image
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!CardManager.instance.IsDashCardActivated() && !CardManager.instance.IsInviCardActivated() && collision.gameObject.CompareTag("LaserCard"))
        {
            CardManager.instance.ActivateLaserCard();
            ActivateLaserPowerUp();
            // collision.gameObject.SetActive(false);
            Destroy(collision.gameObject); // Destroy the power-up on collision
        }
    }
}