using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] public GameObject[] powerUpPrefabs;
    [SerializeField] public float[] spawnProbabilities;
    [SerializeField] public Vector2[] spawnPositions;
    [SerializeField] private float powerUpDuration = 8f;
    [SerializeField] private float spawnInterval = 15f;
    [SerializeField] private float startDelay = 0f;

    public Dictionary<Vector2, GameObject> occupiedSpawnPositions = new Dictionary<Vector2, GameObject>();
    private bool isSpawning = false;

    private void Start()
    {
        StartCoroutine(StartSpawningWithDelay());
    }

    private IEnumerator StartSpawningWithDelay()
    {
        yield return new WaitForSeconds(startDelay);
        isSpawning = true;
        StartCoroutine(SpawnPowerUps());
    }

    private IEnumerator SpawnPowerUps()
    {
        while (isSpawning)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 spawnPoint = GetRandomSpawnPoint();
                GameObject powerUp = Instantiate(ChoosePowerUpPrefab(), spawnPoint, Quaternion.identity);
                occupiedSpawnPositions.Add(spawnPoint, powerUp);
                StartCoroutine(DestroyPowerUpAfterDuration(spawnPoint, powerUpDuration));
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator DestroyPowerUpAfterDuration(Vector2 spawnPoint, float duration)
    {
        yield return new WaitForSeconds(duration);

        if (occupiedSpawnPositions.ContainsKey(spawnPoint))
        {
            Destroy(occupiedSpawnPositions[spawnPoint]);
            occupiedSpawnPositions.Remove(spawnPoint);
        }
    }

    private Vector2 GetRandomSpawnPoint()
    {
        Vector2 spawnPoint;
        do
        {
            int randomIndex = Random.Range(0, spawnPositions.Length);
            spawnPoint = spawnPositions[randomIndex];
        } while (occupiedSpawnPositions.ContainsKey(spawnPoint));

        return spawnPoint;
    }

    private GameObject ChoosePowerUpPrefab()
    {
        float randomValue = Random.value;
        float cumulativeProbability = 0f;

        for (int i = 0; i < powerUpPrefabs.Length; i++)
        {
            cumulativeProbability += spawnProbabilities[i];
            if (randomValue < cumulativeProbability)
            {
                return powerUpPrefabs[i];
            }
        }

        return powerUpPrefabs[powerUpPrefabs.Length - 1];
    }

    // Method to reset the spawner
    public void ResetSpawner()
    {
        StopAllCoroutines(); // Stop all spawning coroutines
        isSpawning = false;
        foreach (var powerUp in occupiedSpawnPositions.Values)
        {
            Destroy(powerUp); // Destroy all spawned power-ups
        }
        occupiedSpawnPositions.Clear(); // Clear the dictionary
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
