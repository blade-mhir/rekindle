using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootItem
{
    public GameObject itemPrefab;
    public float baseDropChance; // Base drop chance value between 0 and 1 (e.g., 0.5 for 50% chance)
    public float vampCardDropChance; // Drop chance when Vamp Card is active
}

public class LootBag : MonoBehaviour
{
    [SerializeField] private List<LootItem> lootItems = new List<LootItem>();

    // This method will be called when the enemy dies
    public void DropLoot()
    {
        foreach (LootItem lootItem in lootItems)
        {
            float effectiveDropChance = VampCardActivation.isVampCardActive ? lootItem.vampCardDropChance : lootItem.baseDropChance;
            float randomValue = Random.value; // Returns a value between 0 and 1

            if (randomValue <= effectiveDropChance)
            {
                Instantiate(lootItem.itemPrefab, transform.position, Quaternion.identity);
                break; // Break out of the loop after dropping one item
            }
        }
    }


}
