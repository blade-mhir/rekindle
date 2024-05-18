using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollectionManager : MonoBehaviour
{
    public static CardCollectionManager Instance;

    [SerializeField] private int cardLimit = 3;
    private HashSet<string> collectedCards = new HashSet<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool CanCollectCard(string cardName)
    {
        return collectedCards.Count < cardLimit && !collectedCards.Contains(cardName);
    }

    public void CollectCard(string cardName)
    {
        if (CanCollectCard(cardName))
        {
            collectedCards.Add(cardName);
        }
    }

    public bool IsCardLimitReached()
    {
        return collectedCards.Count >= cardLimit;
    }
}
