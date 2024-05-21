using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    [SerializeField] private int coinValue = 10;
    private CoinManager coinManager;
    private LootBag lootBag;
    private Animator animator;
    [SerializeField] private Image healthBarFill; // Optional health bar fill image

    public int MaxHealth => maxHealth; // Public getter for maxHealth
    public int CurrentHealth => currentHealth; // Public getter for currentHealth

    private void Awake()
    {
        currentHealth = maxHealth;
        coinManager = FindObjectOfType<CoinManager>();
        lootBag = GetComponent<LootBag>();
        animator = GetComponent<Animator>();
        UpdateHealthBar(); // Initialize the health bar
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        UpdateHealthBar(); // Update the health bar when taking damage

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

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }
}
