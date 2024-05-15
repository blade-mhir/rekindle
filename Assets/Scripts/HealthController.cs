using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
  [SerializeField] private PlayerController playerController; // Assign the PlayerController script
  [SerializeField] private Image healthBarImage; // Assign the health bar image in the Inspector
  [SerializeField] private float maxHealth = 100f; // Now a float for smoother damage

  [SerializeField] private GameObject gameOverObject; // Assign the game over GameObject in the Inspector
  [SerializeField] private float healthPowerUpAmount = 20f; // Amount of health restored by power-up (modify in Inspector)

  private float currentHealth;

  void Start()
  {
    currentHealth = maxHealth;
  }

  public void TakeDamage(float damage)
  {
    currentHealth -= damage;

    if (currentHealth <= 0)
    {
      currentHealth = 0;
      playerController.Die(); // Call player death logic
      gameOverObject.SetActive(true);
    }

    UpdateHealthBar();
  }

  private void UpdateHealthBar()
  {
    healthBarImage.fillAmount = currentHealth / maxHealth;
  }

  void OnCollisionEnter2D(Collision2D collision) // Use OnCollisionEnter2D for collision detection
  {
    if (collision.gameObject.tag == "HP" && currentHealth < maxHealth) // Check for HP tag and not full health
    {
      currentHealth = Mathf.Clamp(currentHealth + healthPowerUpAmount, 0f, maxHealth); // Add health, clamp between 0 and max
      Destroy(collision.gameObject); // Destroy the health power-up object
      UpdateHealthBar();
    }
  }
}
