using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinearTimer : MonoBehaviour
{
    [SerializeField] private Image timerImage;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject victoryText;
    [SerializeField] private float maxTime = 60f;
    [SerializeField] private float startDelay = 0f; // Delay before the timer starts
    private bool timerStarted = false; // Flag to indicate if the timer has started
    private float currentTime;
    private bool isGameOver = false;
    private List<EnemySpawner> enemySpawners = new List<EnemySpawner>();

    private void Start()
    {
        // Populate enemySpawners list by finding all EnemySpawner instances in the scene
        EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>();
        foreach (EnemySpawner spawner in spawners)
        {
            enemySpawners.Add(spawner);
        }

        // Start the timer after the delay
        StartCoroutine(StartTimerWithDelay());
    }

    private IEnumerator StartTimerWithDelay()
    {
        yield return new WaitForSeconds(startDelay);
        timerStarted = true;
        currentTime = maxTime;
    }

    private void Update()
    {
        if (timerStarted && !isGameOver)
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
