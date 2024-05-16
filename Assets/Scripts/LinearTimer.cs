using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinearTimer : MonoBehaviour
{
    [SerializeField] private Image timerImage;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject victoryText;

    // Reference to all EnemySpawner instances
    [SerializeField] private List<EnemySpawner> enemySpawners = new List<EnemySpawner>();

    [SerializeField] private float maxTime = 60f;
    private float currentTime;
    private bool isGameOver = false;

    private void Start()
    {
        currentTime = maxTime;

        // Populate enemySpawners list by finding all EnemySpawner instances in the scene
        EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>();
        foreach (EnemySpawner spawner in spawners)
        {
            enemySpawners.Add(spawner);
        }
    }

    private void Update()
    {
        if (!isGameOver)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0f)
            {
                currentTime = 0f;
                Time.timeScale = 0f;
                isGameOver = true;

                bool anyRemainingEnemies = false;

                // Check each EnemySpawner for remaining enemies
                foreach (EnemySpawner spawner in enemySpawners)
                {
                    if (spawner.HasRemainingEnemies())
                    {
                        anyRemainingEnemies = true;
                        break;
                    }
                }

                // Show game over or victory text based on remaining enemies
                if (anyRemainingEnemies)
                {
                    gameOverText.SetActive(true);
                }
                else
                {
                    victoryText.SetActive(true);
                    // Assuming you want to show the continue button here
                    victoryText.GetComponentInChildren<VictoryContinueMenu>().victoryContinueUI.SetActive(true);
                }
            }

            timerImage.fillAmount = currentTime / maxTime;
        }
    }
}
