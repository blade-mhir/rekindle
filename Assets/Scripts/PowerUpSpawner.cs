using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] powerUpPrefabs; // Array of power up prefabs
    [SerializeField] private float[] spawnProbabilities; // Array of spawn probabilities
    [SerializeField] private Vector2[] spawnPositions; // Array of spawn positions (x, y)
    [SerializeField] private float powerUpDuration = 8f; // Duration of power up
    [SerializeField] private float spawnInterval = 15f; // Interval for respawning power-ups

    private Dictionary<Vector2, GameObject> occupiedSpawnPositions = new Dictionary<Vector2, GameObject>();
    private bool isSpawning = false;

    private void Start()
    {
        // Validate probabilities array
        if (spawnProbabilities.Length != powerUpPrefabs.Length)
        {
            Debug.LogError("Spawn probabilities array length must match power up prefabs array length.");
            return;
        }

        // Normalize probabilities
        float totalProbability = 0f;
        foreach (float prob in spawnProbabilities)
        {
            totalProbability += prob;
        }

        for (int i = 0; i < spawnProbabilities.Length; i++)
        {
            spawnProbabilities[i] /= totalProbability;
        }

        // Start spawning power-ups
        StartCoroutine(SpawnPowerUps());
    }

    private IEnumerator SpawnPowerUps()
    {
        isSpawning = true;

        while (true)
        {
            // Spawn two power-ups
            for (int i = 0; i < 2; i++)
            {
                // Choose a random spawn position
                Vector2 spawnPoint = GetRandomSpawnPoint();

                // Choose a random power up prefab based on probabilities
                GameObject powerUp = Instantiate(ChoosePowerUpPrefab(), spawnPoint, Quaternion.identity);
                occupiedSpawnPositions.Add(spawnPoint, powerUp);

                // Destroy power up after duration
                StartCoroutine(DestroyPowerUpAfterDuration(spawnPoint, powerUpDuration));
            }

            // Wait for the spawn interval before spawning again
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
        float randomValue = Random.value; // Random value between 0 and 1
        float cumulativeProbability = 0f;

        for (int i = 0; i < powerUpPrefabs.Length; i++)
        {
            cumulativeProbability += spawnProbabilities[i];
            if (randomValue < cumulativeProbability)
            {
                return powerUpPrefabs[i];
            }
        }

        // Fallback in case of floating point precision issues
        return powerUpPrefabs[powerUpPrefabs.Length - 1];
    }

    // Call this method to manually start spawning power-ups
    public void StartSpawning()
    {
        if (!isSpawning)
            StartCoroutine(SpawnPowerUps());
    }
}
