using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootItem
{
    public GameObject itemPrefab;
    public float baseDropChance; // Base drop chance value between 0 and 1 (e.g., 0.5 for 50% chance)
    public float vampCardDropChance; // Drop chance when Vampirism Card is active
}

public class LootBag : MonoBehaviour
{
    [SerializeField] private List<LootItem> lootItems = new List<LootItem>();
    private bool isVampCardActive = false;

    // This method will be called when the enemy dies
    public void DropLoot()
    {
        foreach (LootItem lootItem in lootItems)
        {
            float randomValue = Random.value; // Returns a value between 0 and 1
            float dropChance = isVampCardActive ? lootItem.vampCardDropChance : lootItem.baseDropChance;

            if (randomValue <= dropChance)
            {
                Instantiate(lootItem.itemPrefab, transform.position, Quaternion.identity);
            }
        }
    }

    // Method to activate the Vampirism Card
    public void ActivateVampCard()
    {
        isVampCardActive = true;
    }
}
