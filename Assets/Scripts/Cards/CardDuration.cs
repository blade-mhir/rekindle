using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDuration : MonoBehaviour
{
    public float duration = 3.0f; // Duration in seconds

    void Start()
    {
        // Start a coroutine to destroy the prefab after the specified duration
        StartCoroutine(DestroyAfterDuration(duration));
    }

    IEnumerator DestroyAfterDuration(float duration)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Destroy the prefab
        Destroy(gameObject);
    }
}
