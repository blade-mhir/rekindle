using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Include this for UI components

namespace BossNamespace
{
    [System.Serializable]
    public class SpawnDetails
    {
        public GameObject enemyPrefab;
        public int spawnCount;
        public float bossHealthBeforeSpawn; // Health percentage before spawning
        public Vector2 startPoint;
        public Vector2 endPoint;
    }

    public class BossSpawner : MonoBehaviour
    {
        public List<SpawnDetails> spawnDetailsList;
        [SerializeField] private GameObject boss; // Reference to the Boss GameObject
        private EnemyHealth bossHealth; // Reference to the Boss's health component
        private HashSet<SpawnDetails> spawnedDetails = new HashSet<SpawnDetails>(); // Track which spawn details have been used

        private List<GameObject> spawnedEnemies = new List<GameObject>();
        [SerializeField] private GameObject victoryMenu; // Reference to the Victory Menu GameObject
        [SerializeField] private Image healthBar; // Reference to the Health Bar Image

        private void Start()
        {
            if (boss != null)
            {
                bossHealth = boss.GetComponent<EnemyHealth>();
            }

            StartCoroutine(CheckBossHealth());
        }

        private IEnumerator CheckBossHealth()
        {
            while (bossHealth != null && bossHealth.CurrentHealth > 0)
            {
                float healthPercentage = (float)bossHealth.CurrentHealth / bossHealth.MaxHealth;

                // Update the health bar
                if (healthBar != null)
                {
                    healthBar.fillAmount = healthPercentage;
                }

                foreach (var spawnDetails in spawnDetailsList)
                {
                    if (healthPercentage <= spawnDetails.bossHealthBeforeSpawn / 100f && !spawnedDetails.Contains(spawnDetails))
                    {
                        StartCoroutine(SpawnEnemies(spawnDetails));
                        spawnedDetails.Add(spawnDetails);
                    }
                }

                yield return new WaitForSeconds(1f); // Check every second
            }

            if (bossHealth.CurrentHealth <= 0)
            {
                ShowVictoryMenu();
            }
        }

        private IEnumerator SpawnEnemies(SpawnDetails details)
        {
            int spawnedCount = 0;

            while (spawnedCount < details.spawnCount)
            {
                Vector3 spawnPosition = new Vector3(details.startPoint.x, details.startPoint.y, 0);
                GameObject enemy = Instantiate(details.enemyPrefab, spawnPosition, Quaternion.identity);
                spawnedEnemies.Add(enemy);

                StartCoroutine(MoveEnemyToPosition(enemy, new Vector3(details.endPoint.x, details.endPoint.y, 0)));

                spawnedCount++;
                yield return null; // Yield to the next frame to avoid freezing the game
            }
        }

        private IEnumerator MoveEnemyToPosition(GameObject enemy, Vector3 endPosition)
        {
            float duration = 1.0f;
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

        private void ShowVictoryMenu()
        {
            if (victoryMenu != null)
            {
                victoryMenu.SetActive(true);
            }
        }
    }
}
