using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
  [SerializeField] private int maxHealth = 100; // Configure enemy health in the inspector
  private int currentHealth;

  public void Awake()
  {
    currentHealth = maxHealth;
  }

  public void TakeDamage(int damageAmount)
  {
    currentHealth -= damageAmount;

    // Handle enemy death (optional)
    if (currentHealth <= 0)
    {
      // Destroy the enemy gameObject (or play a death animation)
      Destroy(gameObject);
    }
  }
}

