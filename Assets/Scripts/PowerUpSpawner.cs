using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private PowerupData[] powerupPrefabs; // Array of powerup data
    [SerializeField] private float[] lanePositions; // Array of x-axis positions for each lane
    [SerializeField] private float spawnDelay = 2.0f; // Delay between spawns (seconds)

    private float nextSpawnTime;

    private void Start()
    {
        nextSpawnTime = Time.time + spawnDelay;
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnPowerup();
            nextSpawnTime = Time.time + spawnDelay;
        }
    }

    private void SpawnPowerup()
    {
        // Choose a random powerup based on spawn chances
        float totalChance = 0;
        foreach (var powerupData in powerupPrefabs)
        {
            totalChance += powerupData.spawnChance;
        }

        float randomValue = Random.value * totalChance;
        float accumulatedChance = 0;
        int selectedIndex = 0;

        while (accumulatedChance < randomValue)
        {
            accumulatedChance += powerupPrefabs[selectedIndex].spawnChance;
            selectedIndex++;
        }

        // Choose a random lane position
        int randomLaneIndex = Random.Range(0, lanePositions.Length - 1);
        float randomXPosition = lanePositions[randomLaneIndex];

        // Create a spawn position vector based on the chosen lane position
        Vector3 spawnPosition = new Vector3(randomXPosition, transform.position.y, transform.position.z);

        // Spawn the powerup at the calculated spawn position
        GameObject powerup = Instantiate(powerupPrefabs[selectedIndex].prefab, spawnPosition, transform.rotation);
    }
}

[System.Serializable]
public class PowerupData
{
    public GameObject prefab; // Prefab of the powerup
    [Range(0f, 1f)] public float spawnChance; // Chance of spawning this powerup (0 to 1)
}
