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
        if (cooldownFillImage != null)
        {
            cooldownFillImage.fillAmount = 0f; // Ensure the cooldown fill image is initially empty
        }
    }

    private void Update()
    {
        // Activate laser powerup with "E" key
        if (Input.GetKeyDown(KeyCode.E) && !isCooldownActive && CardManager.instance.IsLaserCardActivated())
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
        // Enable the laser power-up object
        if (laserPowerUpObject != null)
        {
            laserPowerUpObject.SetActive(true);
        }
        isLaserActive = true;
        laserStartTime = Time.time;

        shootingScript.SetLaserBulletPrefab(laserBulletPrefab);

        // Start cooldown coroutine
    }

    private void DeactivateLaserPowerUp()
    {
        isLaserActive = false;

        shootingScript.ResetBulletPrefab();
        StartCoroutine(CooldownCoroutine());
    
    }

    private IEnumerator CooldownCoroutine()
    {
        isCooldownActive = true;

        if (cooldownFillImage != null)
        {
            cooldownFillImage.gameObject.SetActive(true);
            float fillAmount = 1f;
            float fillChangeRate = 1f / cooldownDuration;
            // cooldownFillImage.fillAmount = 1f - cooldownFillAmount;

            while (fillAmount > 0)
            {
                fillAmount -= fillChangeRate * Time.deltaTime;
                cooldownFillImage.fillAmount = fillAmount;
                yield return null;
            }

            cooldownFillImage.gameObject.SetActive(false);
        }

        isCooldownActive = false;
        //cooldownStartTime = Time.time;

        // while (Time.time < cooldownStartTime + cooldownDuration)
        // {
        //     float timeSinceCooldownStart = Time.time - cooldownStartTime;
        //     float cooldownFillAmount = Mathf.Clamp01(timeSinceCooldownStart / cooldownDuration);
        //     if (cooldownFillImage != null)
        //     {
        //         cooldownFillImage.fillAmount = 1f - cooldownFillAmount;
        //     }

        //     yield return null; // Wait for the next frame
        // }

        // isCooldownActive = false;
        // if (cooldownFillImage != null)
        // {
        //     cooldownFillImage.fillAmount = 0f; // Reset cooldown fill image
        // }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!CardManager.instance.IsDashCardActivated() && !CardManager.instance.IsInviCardActivated() && collision.gameObject.CompareTag("LaserCard"))
        {
            CardManager.instance.ActivateLaserCard();
            if (laserPowerUpObject != null)
            {
                laserPowerUpObject.SetActive(true);
            }
            Destroy(collision.gameObject); // Destroy the power-up on collision
        }
    }
}
