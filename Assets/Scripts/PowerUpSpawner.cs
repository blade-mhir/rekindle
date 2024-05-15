using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] powerUpPrefabs; // Array of power up prefabs
    [SerializeField] private float[] spawnProbabilities; // Array of spawn probabilities
    [SerializeField] private int numberOfSpawns; // Number of power ups to spawn
    [SerializeField] private Vector2[] spawnPositions; // Array of spawn positions (x, y)
    [SerializeField] private float powerUpDuration = 8f; // Duration of power up

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
        SpawnPowerUps();
    }

    private void SpawnPowerUps()
    {
        for (int i = 0; i < numberOfSpawns; i++)
        {
            // Choose a random spawn position
            int randomIndex = Random.Range(0, spawnPositions.Length);
            Vector2 spawnPoint = spawnPositions[randomIndex];

            // Choose a random power up prefab based on probabilities
            GameObject powerUp = Instantiate(ChoosePowerUpPrefab(), spawnPoint, Quaternion.identity);

            // Destroy power up after duration
            Destroy(powerUp, powerUpDuration);
        }
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
}
