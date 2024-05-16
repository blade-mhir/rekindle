using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController; // Assign the PlayerController script
    [SerializeField] private GameObject gameOverObject; // Assign the game over GameObject in the Inspector
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private Image healthBarImage; // Assign the health bar image in the Inspector
    [SerializeField] private float maxHealth = 100f; // Now a float for smoother damage
    
    [SerializeField] private float healthPowerUpAmount = 20f; // Amount of health restored by power-up (modify in Inspector)

    [SerializeField] private Image shieldBarImage; // Assign the shield bar image in the Inspector
    [SerializeField] private float maxShield = 50f; // Maximum shield value

    private float currentHealth;
    private float currentShield;

    void Start()
    {
        currentHealth = maxHealth;
        currentShield = maxShield;
        UpdateHealthBar();
        UpdateShieldBar();
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

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            playerController.Die(); // Call player death logic
            gameOverObject.SetActive(true);
            // Trigger isDead animation
            GetComponent<Animator>().SetBool("isDead", true);
            // Destroy all enemies when the player dies
            enemyManager.DestroyAllEnemies();

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
    }
}
