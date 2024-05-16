using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    [SerializeField] private int coinValue = 10; // Coin value of this enemy
    private CoinManager coinManager; // Reference to the Coin Manager

    private LootBag lootBag;

    private void Awake()
    {
        currentHealth = maxHealth;
        lootBag = GetComponent<LootBag>();
        coinManager = FindObjectOfType<CoinManager>(); // Find the Coin Manager in the scene
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("isDead");
                if (lootBag != null)
                {
                    lootBag.DropLoot();
                }
                StartCoroutine(DestroyAfterAnimation());
            }
            else
            {
                if (lootBag != null)
                {
                    lootBag.DropLoot();
                }
                Destroy(gameObject);
            }

            // Add coin value to the Coin Manager
            if (coinManager != null)
            {
                coinManager.AddCoins(coinValue);
            }
        }
        else
        {
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("isHurting");
            }
        }
    }

    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public int GetCoinValue()
    {
        return coinValue;
    }
}