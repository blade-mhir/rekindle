using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SpawnDetails
{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public float startTime; // Time when spawning starts
    public float endTime; // Time when spawning ends
    public int spawnCount; // Number of enemies to spawn in the given interval
}

public class EnemySpawner : MonoBehaviour
{
    public List<SpawnDetails> spawnDetailsList; // List of spawn details
    public Vector2 startPoint; // Starting position where the enemies will enter
    public Vector2 endPoint; // Ending position where the enemies will move to

    private List<GameObject> spawnedEnemies = new List<GameObject>(); // Track spawned enemies

    private void Start()
    {
        foreach (var spawnDetails in spawnDetailsList)
        {
            StartCoroutine(SpawnEnemies(spawnDetails));
        }
    }

    private IEnumerator SpawnEnemies(SpawnDetails details)
    {
        float spawnWindow = details.endTime - details.startTime;
        int spawnedCount = 0;

        // Wait until the start time
        yield return new WaitForSeconds(details.startTime);

        while (spawnedCount < details.spawnCount)
        {
            float randomDelay = Random.Range(0f, spawnWindow);
            spawnWindow -= randomDelay;

            // Instantiate the enemy at the specified start point
            Vector3 spawnPosition = new Vector3(startPoint.x, startPoint.y, 0);
            GameObject enemy = Instantiate(details.enemyPrefab, spawnPosition, Quaternion.identity);
            spawnedEnemies.Add(enemy); // Add the spawned enemy to the list

            // Move the enemy to the end point
            StartCoroutine(MoveEnemyToPosition(enemy, new Vector3(endPoint.x, endPoint.y, 0)));

            spawnedCount++;
            yield return new WaitForSeconds(randomDelay);
        }
    }

    private IEnumerator MoveEnemyToPosition(GameObject enemy, Vector3 endPosition)
    {
        float duration = 1.0f; // Adjust the duration as needed
        float elapsedTime = 0;
        Vector3 startingPosition = enemy.transform.position;

        while (elapsedTime < duration)
        {
            enemy.transform.position = Vector3.Lerp(startingPosition, endPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemy.transform.position = endPosition;
    }

    public bool HasRemainingEnemies()
    {
        // Check if there are any active enemies in the spawnedEnemies list
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                return true; // If at least one enemy is alive, return true
            }
        }
        return false; // If no enemies are alive, return false
    }
}
