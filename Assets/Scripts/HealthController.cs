using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController; // Assign the PlayerController script
    [SerializeField] private GameOverMenu gameOverMenu; // Assign the GameOverMenu script
    [SerializeField] private float maxHealth = 100f; // Base maximum health
    [SerializeField] private float maxShield = 50f; // Base maximum shield
    [SerializeField] private float healthPowerUpAmount = 20f; // Amount of health restored by power-up (modify in Inspector)
    [SerializeField] private GameObject healthBarGameObject; // Assign the HealthBar GameObject in the Inspector
    [SerializeField] private Image healthBarImage; // Assign the health bar image in the Inspector
    [SerializeField] private GameObject shieldBarGameObject; // Assign the ShieldBar GameObject in the Inspector
    [SerializeField] private Image shieldBarImage; // Assign the shield bar image in the Inspector
    [SerializeField] private GameObject newHealthBarGameObject; // Assign the new HealthBar GameObject in the Inspector
    [SerializeField] private Image newHealthBarImage; // Assign the new HealthBar Image in the Inspector
    [SerializeField] private GameObject newShieldBarGameObject; // Assign the new ShieldBar GameObject in the Inspector
    [SerializeField] private Image newShieldBarImage; // Assign the new ShieldBar Image in the Inspector
    [SerializeField] private GameObject hPCardGameObject; // Assign the HP Card GameObject in the Inspector
    [SerializeField] private GameObject shieldCardGameObject; // Assign the Shield Card GameObject in the Inspector
    [SerializeField] private GameObject hPotionCardGameObject; // Assign the Health Potion Card GameObject in the Inspector
    [SerializeField] private GameObject hpotionEffect1; // Assign the first effect GameObject in the Inspector
    [SerializeField] private GameObject hpotionEffect2; // Assign the second effect GameObject in the Inspector

    private float currentHealth;
    private float currentShield;
    private bool hasHealthPotion = false; // Variable to track if the player has a health potion

    private void Awake()
    {
        Debug.Log("HealthController Awake");
        // Ensure the health and shield are initialized properly
        ResetHealthState();
    }

    void Start()
    {
        InitializeHealthAndShield();
    }

    public void InitializeReferences(GameOverMenu newGameOverMenu)
    {
        gameOverMenu = newGameOverMenu;
    }

    private void InitializeHealthAndShield()
    {
        Debug.Log("Initializing Health and Shield");
        currentHealth = maxHealth;
        currentShield = maxShield;
        UpdateHealthBar();
        UpdateShieldBar();

         // Debug statement to check if playerController is assigned
        if (playerController == null)
        {
            Debug.LogError("PlayerController is not assigned!");
        }


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

        // Deactivate new health and shield bar game objects initially
        if (newHealthBarGameObject != null)
        {
            newHealthBarGameObject.SetActive(false);
        }
        if (newShieldBarGameObject != null)
        {
            newShieldBarGameObject.SetActive(false);
        }

            // Debug statement to check if healthBarImage is assigned
        if (healthBarImage == null)
        {
            Debug.LogError("healthBarImage is not assigned!");
        }
        else if (!healthBarImage.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("healthBarImage is inactive!");
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
            gameOverMenu.ShowGameOverMenu(); // Show Game Over Menu
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
        // Debug logging before accessing healthBarImage
        if (healthBarImage == null)
        {
            Debug.LogWarning("healthBarImage is null!");
            return;
        }
        else if (!healthBarImage.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("healthBarImage is inactive!");
            return;
        }

        // Calculate the fill amount based on current health and max health
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

            // Activate the two additional effects
            if (hpotionEffect1 != null)
            {
                hpotionEffect1.SetActive(true);
            }
            if (hpotionEffect2 != null)
            {
                hpotionEffect2.SetActive(true);
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

        // Deactivate old health bar game object and image
        if (healthBarGameObject != null)
        {
            healthBarGameObject.SetActive(false);
        }
        if (healthBarImage != null)
        {
            healthBarImage.gameObject.SetActive(false);
        }

        // Activate the new health bar game object and image
        if (newHealthBarGameObject != null)
        {
            newHealthBarGameObject.SetActive(true);
        }
        if (newHealthBarImage != null)
        {
            newHealthBarImage.gameObject.SetActive(true);
            healthBarImage = newHealthBarImage; // Update the reference to the new health bar image
        }

        // Update the health bar to reflect the new max health
        UpdateHealthBar();
    }

    private void IncreaseBaseMaxShield()
    {
        maxShield += 1; // Increase base maximum shield by 1
        currentShield = maxShield; // Fully replenish shield to the new max shield

        // Deactivate old shield bar game object and image
        if (shieldBarGameObject != null)
        {
            shieldBarGameObject.SetActive(false);
        }
        if (shieldBarImage != null)
        {
            shieldBarImage.gameObject.SetActive(false);
        }

        // Activate the new shield bar game object and image
        if (newShieldBarGameObject != null)
        {
            newShieldBarGameObject.SetActive(true);
        }
        if (newShieldBarImage != null)
        {
            newShieldBarImage.gameObject.SetActive(true);
            shieldBarImage = newShieldBarImage; // Update the reference to the new shield bar image
        }

        // Update the shield bar to reflect the new max shield
        UpdateShieldBar();
    }

    // Call this method when the game restarts to reset health and shield
    public void ResetHealthState()
    {
        maxHealth = 3f; // Reset base maximum health
        maxShield = 2f; // Reset base maximum shield
        currentHealth = maxHealth;
        currentShield = maxShield;
        hasHealthPotion = false;

        // Reset UI elements to their initial state
        InitializeHealthAndShield();
        UpdateHealthBar();
        UpdateShieldBar();
    }
}
