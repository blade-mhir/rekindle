using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    [SerializeField] private int coinValue = 10;
    private CoinManager coinManager;
    private LootBag lootBag;
    private Animator animator;

    private void Awake()
    {
        currentHealth = maxHealth;
        coinManager = FindObjectOfType<CoinManager>();
        lootBag = GetComponent<LootBag>();
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (animator != null)
            {
                animator.SetTrigger("isHurting");
            }
        }
    }

    private void Die()
    {
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

        if (coinManager != null)
        {
            coinManager.AddCoins(coinValue);
        }
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public int GetCoinValue()
    {
        return coinValue;
    }

    // Method to reset enemy health
    public void ResetEnemyHealth()
    {
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        GameManager.OnGameOver += ResetEnemyHealth; // Subscribe to the GameManager's OnGameOver event
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= ResetEnemyHealth; // Unsubscribe from the GameManager's OnGameOver event
    }
}
