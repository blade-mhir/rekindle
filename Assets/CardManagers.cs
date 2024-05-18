using System.Collections.Generic;
using UnityEngine;

public class CardManagers : MonoBehaviour
{
    public static CardManagers instance;

    [SerializeField] private int maxCollectableCards = 3; // Maximum number of cards that can be collected (customizable in the Inspector)
    private List<string> collectedCards = new List<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool CanCollectCard(string cardTag)
    {
        return collectedCards.Count < maxCollectableCards && !collectedCards.Contains(cardTag);
    }

    public void CollectCard(string cardTag)
    {
        if (CanCollectCard(cardTag))
        {
            collectedCards.Add(cardTag);
        }
    }

    public bool IsCardCollected(string cardTag)
    {
        return collectedCards.Contains(cardTag);
    }
}
