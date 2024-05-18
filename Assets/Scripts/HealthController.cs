using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
   [SerializeField] private PlayerController playerController; // Assign the PlayerController script
    [SerializeField] private GameObject gameOverObject; // Assign the game over GameObject in the Inspector

    [SerializeField] private float baseMaxHealth = 100f; // Base maximum health
    [SerializeField] private float baseMaxShield = 50f; // Base maximum shield
    [SerializeField] private float healthPowerUpAmount = 20f; // Amount of health restored by power-up (modify in Inspector)
    [SerializeField] private GameObject healthBarGameObject; // Assign the HealthBar GameObject in the Inspector
    [SerializeField] private Image healthBarImage; // Assign the health bar image in the Inspector
    [SerializeField] private GameObject shieldBarGameObject; // Assign the ShieldBar GameObject in the Inspector
    [SerializeField] private Image shieldBarImage; // Assign the shield bar image in the Inspector
    [SerializeField] private GameObject newHealthBarGameObject; // Assign the new HealthBar GameObject in the Inspector
    [SerializeField] private Image newHealthBarImage; // Assign the new HealthBar Image in the Inspector
    [SerializeField] private GameObject newShieldBarGameObject; // Assign the new ShieldBar GameObject in the Inspector
    [SerializeField] private Image newShieldBarImage; // Assign the new ShieldBar Image in the Inspector

    [SerializeField] private GameObject hPotionCardGameObject; // Assign the Health Potion Card GameObject in the Inspector
    [SerializeField] private GameObject hPCardGameObject; // Assign the HP Card GameObject in the Inspector
    [SerializeField] private GameObject shieldCardGameObject; // Assign the Shield Card GameObject in the Inspector

    private float maxHealth;
    private float maxShield;
    private float currentHealth;
    private float currentShield;
    private bool hasHealthPotion = false; // Variable to track if the player has a health potion

    private void OnEnable()
    {
        GameManager.OnGameOver += ResetHealthState; // Subscribe to the GameManager's OnGameOver event
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= ResetHealthState; // Unsubscribe from the GameManager's OnGameOver event
    }

    private void Awake()
    {
        ResetHealthState();
    }

    private void ResetHealthState()
    {
        maxHealth = baseMaxHealth;
        maxShield = baseMaxShield;
        currentHealth = maxHealth;
        currentShield = maxShield;
        hasHealthPotion = false;

        // Reset UI elements
        UpdateHealthBar();
        UpdateShieldBar();

        // Activate health bar and shield bar game objects
        if (healthBarGameObject != null)
        {
            healthBarGameObject.SetActive(true);
        }
        if (shieldBarGameObject != null)
        {
            shieldBarGameObject.SetActive(true);
        }

        // Activate health bar and shield bar images
        if (healthBarImage != null)
        {
            healthBarImage.gameObject.SetActive(true);
        }
        if (shieldBarImage != null)
        {
            shieldBarImage.gameObject.SetActive(true);
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentShield = maxShield;
        UpdateHealthBar();
        UpdateShieldBar();

        // Activate health bar and shield bar game objects
        if (healthBarGameObject != null)
        {
            healthBarGameObject.SetActive(true);
        }
        if (shieldBarGameObject != null)
        {
            shieldBarGameObject.SetActive(true);
        }

        // Activate health bar and shield bar images
        if (healthBarImage != null)
        {
            healthBarImage.gameObject.SetActive(true);
        }
        if (shieldBarImage != null)
        {
            shieldBarImage.gameObject.SetActive(true);
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentShield > 0)
        {
            currentShield -= damage;

            if (currentShield < 0)
            {
                currentHealth += currentShield; // Deduct the remaining damage from health
                currentShield = 0;
            }

            // Update shield bar
            UpdateShieldBar();
        }
        else
        {
            currentHealth -= damage;
        }

        if (currentHealth <= 1 && hasHealthPotion)
        {
            UseHealthPotion();
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            playerController.Die(); // Call player death logic
            gameOverObject.SetActive(true);
            // Freeze the game
            Time.timeScale = 0f;
        }
        else
        {
            // Trigger isHurting animation
            GetComponent<Animator>().SetBool("isHurt", true);
            StartCoroutine(ResetHurtAnimation()); // Reset after a short duration
        }

        UpdateHealthBar();
    }

    IEnumerator ResetHurtAnimation()
    {
        yield return new WaitForSeconds(0.2f); // Wait for 0.2 seconds (adjust as needed)
        GetComponent<Animator>().SetBool("isHurt", false);
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = currentHealth / maxHealth;
    }

    private void UpdateShieldBar()
    {
        shieldBarImage.fillAmount = currentShield / maxShield;
    }

    void OnCollisionEnter2D(Collision2D collision) // Use OnCollisionEnter2D for collision detection
    {
        if (CardCollectionManager.Instance.IsCardLimitReached())
        {
        // Optionally: Add logic to disable collision with specific tags or give feedback
        return;
        }

        if (collision.gameObject.tag == "HP")
        {
            if (currentShield == 0 && currentHealth < maxHealth)
            {
                currentHealth = Mathf.Clamp(currentHealth + healthPowerUpAmount, 0f, maxHealth); // Add health, clamp between 0 and max
                Destroy(collision.gameObject); // Destroy the health power-up object
                UpdateHealthBar();
            }
        }
        else if (collision.gameObject.tag == "HPotionCard")
        {
            if (CardCollectionManager.Instance.CanCollectCard("HPotionCard"))
            {
                hasHealthPotion = true; // Player now has a health potion
                CardCollectionManager.Instance.CollectCard("HPotionCard");
                Destroy(collision.gameObject); // Destroy the health potion object
                if (hPotionCardGameObject != null)
                {
                    hPotionCardGameObject.SetActive(true); // Activate health potion card game object
                }
            }
        }
        else if (collision.gameObject.tag == "HPCard")
        {
            if (CardCollectionManager.Instance.CanCollectCard("HPCard"))
            {
                IncreaseBaseMaxHealth();
                CardCollectionManager.Instance.CollectCard("HPCard");
                Destroy(collision.gameObject); // Destroy the HP Card object
                if (hPCardGameObject != null)
                {
                    hPCardGameObject.SetActive(true); // Activate HP card game object
                }
            }
        }
        else if (collision.gameObject.tag == "ShieldCard")
        {
            if (CardCollectionManager.Instance.CanCollectCard("ShieldCard"))
            {
                IncreaseBaseMaxShield();
                CardCollectionManager.Instance.CollectCard("ShieldCard");
                Destroy(collision.gameObject); // Destroy the Shield Card object
                if (shieldCardGameObject != null)
                {
                    shieldCardGameObject.SetActive(true); // Activate shield card game object
                }
            }
        }
    }

    private void UseHealthPotion()
    {
        currentHealth = maxHealth; // Fully replenish health
        hasHealthPotion = false; // Use up the potion
        if (hPotionCardGameObject != null)
        {
            hPotionCardGameObject.SetActive(false); // Deactivate the health potion card game object
        }
        UpdateHealthBar();
    }

    private void IncreaseBaseMaxHealth()
    {
        maxHealth += 1; // Increase base maximum health by 1
        currentHealth = maxHealth; // Fully replenish health to the new max health

        // Replace health bar game object and image
        if (healthBarGameObject != null)
        {
            healthBarGameObject.SetActive(false); // Deactivate the old health bar game object
        }
        if (healthBarImage != null)
        {
            healthBarImage.gameObject.SetActive(false); // Deactivate the old health bar image
        }
        if (newHealthBarGameObject != null)
        {
            newHealthBarGameObject.SetActive(true); // Activate the new health bar game object
        }
        if (newHealthBarImage != null)
        {
            newHealthBarImage.gameObject.SetActive(true); // Activate the new health bar image
            healthBarImage = newHealthBarImage; // Update the reference to the new health bar image
        }

        UpdateHealthBar(); // Update the health bar to reflect the new max health
    }

    private void IncreaseBaseMaxShield()
    {
        maxShield += 1; // Increase base maximum shield by 1
        currentShield = maxShield; // Fully replenish shield to the new max shield

        // Replace shield bar game object and image
        if (shieldBarGameObject != null)
        {
            shieldBarGameObject.SetActive(false); // Deactivate the old shield bar game object
        }
        if (shieldBarImage != null)
        {
            shieldBarImage.gameObject.SetActive(false); // Deactivate the old shield bar image
        }
        if (newShieldBarGameObject != null)
        {
            newShieldBarGameObject.SetActive(true); // Activate the new shield bar game object
        }
        if (newShieldBarImage != null)
        {
            newShieldBarImage.gameObject.SetActive(true); // Activate the new shield bar image
            shieldBarImage = newShieldBarImage; // Update the reference to the new shield bar image
        }

        UpdateShieldBar(); // Update the shield bar to reflect the new max shield
    }
    
}
