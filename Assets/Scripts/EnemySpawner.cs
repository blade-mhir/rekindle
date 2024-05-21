using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SpawnDetails
{
    public GameObject enemyPrefab;

    public float startTime;
    public float endTime;
    public int spawnCount;
}

public class EnemySpawner : MonoBehaviour
{
    public List<SpawnDetails> spawnDetailsList;
    public Vector2 startPoint;
    public Vector2 endPoint;
   
    [SerializeField] private float startDelay = 10f;

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(SpawnAllEnemiesWithDelay());
    }

    private IEnumerator SpawnAllEnemiesWithDelay()
    {
        yield return new WaitForSeconds(startDelay);

        foreach (var spawnDetails in spawnDetailsList)
        {
            StartCoroutine(SpawnEnemies(spawnDetails));
        }
    }

    private IEnumerator SpawnEnemies(SpawnDetails details)
    {
        float spawnWindow = details.endTime - details.startTime;
        int spawnedCount = 0;

        yield return new WaitForSeconds(details.startTime);

        while (spawnedCount < details.spawnCount)
        {
            float randomDelay = Random.Range(0f, spawnWindow);
            spawnWindow -= randomDelay;

            Vector3 spawnPosition = new Vector3(startPoint.x, startPoint.y, 0);
            GameObject enemy = Instantiate(details.enemyPrefab, spawnPosition, Quaternion.identity);
            spawnedEnemies.Add(enemy);

            StartCoroutine(MoveEnemyToPosition(enemy, new Vector3(endPoint.x, endPoint.y, 0)));

            spawnedCount++;
            yield return new WaitForSeconds(randomDelay);
        }
    }

    private IEnumerator MoveEnemyToPosition(GameObject enemy, Vector3 endPosition)
    {
        if (enemy != null) 
        {
            float duration = 1.0f;
            float elapsedTime = 0;
            Vector3 startingPosition = enemy.transform.position;

            while (elapsedTime < duration)
            {
                if (enemy != null) 
                {
                    enemy.transform.position = Vector3.Lerp(startingPosition, endPosition, (elapsedTime / duration));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }

            enemy.transform.position = endPosition;
        }
       
    }

    public bool HasRemainingEnemies()
    {
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                return true;
            }
        }
        return false;
    }

    // Method to reset the spawner
    public void ResetSpawner()
    {
        print("Resetting Spawner");
        // Destroy all spawned enemies
        foreach (var enemy in spawnedEnemies)
        {
            Destroy(enemy);
        }
        spawnedEnemies.Clear(); // Clear the list of spawned enemies
        spawnDetailsList.Clear();
    }

    private void OnEnable()
    {
        GameOverMenu.OnGameRestart += ResetSpawner; // Subscribe to the GameManager's OnGameOver event
    }

    private void OnDisable()
    {
        GameOverMenu.OnGameRestart -= ResetSpawner; // Unsubscribe from the GameManager's OnGameOver event
    }
}
