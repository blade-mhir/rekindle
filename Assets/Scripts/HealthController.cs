using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController; // Assign the PlayerController script
    [SerializeField] private Image healthBarImage; // Assign the health bar image in the Inspector
    [SerializeField] private int maxHealth = 100; // Initial health value (adjustable in the Inspector)
    [SerializeField] private GameObject gameOverObject; // Assign the game over GameObject in the Inspector

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

public void TakeDamage(int damage)
{
    currentHealth -= damage;

    if (currentHealth <= 0)
    {
        currentHealth = 0;
        playerController.Die(); // Call player death logic

        // Show game over screen using a reference to the game over GameObject
        gameOverObject.SetActive(true);
    }

    UpdateHealthBar();
}


    private void UpdateHealthBar()
    {
        // Update health bar fill amount based on current health
        healthBarImage.fillAmount = (float)currentHealth / maxHealth;
    }
}
