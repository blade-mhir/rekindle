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
      // Get the Animator component
      Animator animator = GetComponent<Animator>();

      // If Animator exists, trigger "isDead" animation
      if (animator != null)
      {
        animator.SetTrigger("isDead");

        // Destroy the object after the animation finishes (optional)
        StartCoroutine(DestroyAfterAnimation());
      }
      else
      {
        // Destroy immediately if no Animator
        Destroy(gameObject);
      }
    }
    else
    {
      // Trigger the isHurting animation
      Animator animator = GetComponent<Animator>();
      if (animator != null)
      {
        animator.SetTrigger("isHurting");
      }
    }
  }

  IEnumerator DestroyAfterAnimation()
  {
    // Wait for the animation to finish (replace with your animation duration logic)
    yield return new WaitForSeconds(0.5f); // Adjust this time based on your animation length

    // Destroy the enemy GameObject
    Destroy(gameObject);
  }

}

