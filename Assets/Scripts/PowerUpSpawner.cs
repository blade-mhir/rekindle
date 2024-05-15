using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] powerUpPrefabs; // Array of power up prefabs
    [SerializeField] private float[] spawnProbabilities; // Array of spawn probabilities
    [SerializeField] private int numberOfSpawns; // Number of power ups to spawn
    [SerializeField] private Vector2[] spawnPositions; // Array of spawn positions (x, y)
    [SerializeField] private float powerUpDuration = 8f; // Duration of power up
    [SerializeField] private float respawnDelay = 1f; // Delay before respawning a power-up in the same position

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

        // Spawn initial power ups
        StartCoroutine(SpawnPowerUps());
    }

    private IEnumerator SpawnPowerUps()
    {
        isSpawning = true;
        for (int i = 0; i < numberOfSpawns; i++)
        {
            // Choose a random spawn position
            Vector2 spawnPoint = GetRandomSpawnPoint();

            // Choose a random power up prefab based on probabilities
            GameObject powerUp = Instantiate(ChoosePowerUpPrefab(), spawnPoint, Quaternion.identity);
            occupiedSpawnPositions.Add(spawnPoint, powerUp);

            // Wait for power-up duration or activation
            yield return new WaitForSeconds(powerUpDuration);

            // Destroy power up after duration
            if (occupiedSpawnPositions.ContainsKey(spawnPoint))
            {
                Destroy(occupiedSpawnPositions[spawnPoint]);
                occupiedSpawnPositions.Remove(spawnPoint);
            }

            // Wait before respawning
            yield return new WaitForSeconds(respawnDelay);
        }
        isSpawning = false;
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


//ALL 4 positions occupied
// private IEnumerator SpawnPowerUps()
// {
//     isSpawning = true;
//     for (int i = 0; i < spawnPositions.Length; i++)
//     {
//         // Choose a random power up prefab based on probabilities
//         GameObject powerUp = Instantiate(ChoosePowerUpPrefab(), spawnPositions[i], Quaternion.identity);
//         occupiedSpawnPositions.Add(spawnPositions[i], powerUp);

//         // Wait for power-up duration or activation
//         yield return new WaitForSeconds(powerUpDuration);

//         // Destroy power up after duration
//         if (occupiedSpawnPositions.ContainsKey(spawnPositions[i]))
//         {
//             Destroy(occupiedSpawnPositions[spawnPositions[i]]);
//             occupiedSpawnPositions.Remove(spawnPositions[i]);
//         }

//         // Wait before respawning
//         yield return new WaitForSeconds(respawnDelay);
//     }
//     isSpawning = false;
// }

