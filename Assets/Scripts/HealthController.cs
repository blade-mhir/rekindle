using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController; // Assign the PlayerController script
    [SerializeField] private GameObject gameOverObject; // Assign the game over GameObject in the Inspector
    
    [SerializeField] private float maxHealth = 100f; // Now a float for smoother damage
    [SerializeField] private float healthPowerUpAmount = 20f; // Amount of health restored by power-up (modify in Inspector)
    [SerializeField] private float maxShield = 50f; // Maximum shield value
    [SerializeField] private GameObject healthBarGameObject; // Assign the HealthBar GameObject in the Inspector
    [SerializeField] private Image healthBarImage; // Assign the health bar image in the Inspector
    [SerializeField] private GameObject shieldBarGameObject; // Assign the ShieldBar GameObject in the Inspector
    [SerializeField] private Image shieldBarImage; // Assign the shield bar image in the Inspector
    [SerializeField] private GameObject newHealthBarGameObject; // Assign the new HealthBar GameObject in the Inspector
    [SerializeField] private Image newHealthBarImage; // Assign the new HealthBar Image in the Inspector
    [SerializeField] private GameObject newShieldBarGameObject; // Assign the new ShieldBar GameObject in the Inspector
    [SerializeField] private Image newShieldBarImage; // Assign the new ShieldBar Image in the Inspector

    private float currentHealth;
    private float currentShield;
    private bool hasHealthPotion = false; // Variable to track if the player has a health potion

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
            // Trigger isDead animation
            GetComponent<Animator>().SetBool("isDead", true);

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
            hasHealthPotion = true; // Player now has a health potion
            Destroy(collision.gameObject); // Destroy the health potion object
        }
        else if (collision.gameObject.tag == "HPCard")
        {
            IncreaseBaseMaxHealth();
            Destroy(collision.gameObject); // Destroy the HP Card object
        }
        else if (collision.gameObject.tag == "ShieldCard")
        {
            IncreaseBaseMaxShield();
            Destroy(collision.gameObject); // Destroy the Shield Card object
        }
    }

    private void UseHealthPotion()
    {
        currentHealth = maxHealth; // Fully replenish health
        hasHealthPotion = false; // Use up the potion
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
