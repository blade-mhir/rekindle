using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameOverMenu : MonoBehaviour
{
    public static GameOverMenu instance;
    public GameObject gameOverUI; // Reference to the Game Over UI object
    public AudioSource gameOverSound; // Reference to the AudioSource for the Game Over sound
    public CoinManager coinManager; // Reference to the CoinManager script
    public TMP_Text finalScoreText; // Reference to a TextMesh Pro TMP_Text component to display the final score
    public HealthController healthController; // Reference to the HealthController script
    public static event System.Action OnGameRestart;

    public int continueSceneIndex; // Scene index for continue button

    private bool isGameOver = false;

    private void Start()
    {
        gameOverUI.SetActive(false); // Ensure the game over UI is hidden initially
    }

    public void ShowGameOverMenu()
    {
        // Freeze the game
        Time.timeScale = 0f;
        isGameOver = true;

        // Show Game Over menu
        gameOverUI.SetActive(true);

        // Display final score
        DisplayFinalScore();

        // Play Game Over sound
        if (gameOverSound != null)
        {
            gameOverSound.Play();
        }
    }

    void DisplayFinalScore()
    {
        // Get the final score from the CoinManager
        int finalScore = coinManager.coinScore;

        // Display the final score
        finalScoreText.text = "Score: " + finalScore.ToString();

        // Store final score for the next scene
        PlayerPrefs.SetInt("FinalScore", finalScore);
    }

    public void Restart()
    {
        if (isGameOver) // Only proceed if game over
        {
            // Unfreeze the game
            Time.timeScale = 1f;

            if (healthController != null)
            {
                healthController.ResetHealthState();
            }

            gameOverUI.SetActive(false);

            // Load the scene at Level 1 
            SceneManager.LoadScene(1);

            //Load Scene at Present Level
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            OnGameRestart?.Invoke();
        }
    }

    public void MainMenu()
    {
        if (isGameOver) // Only proceed if game over
        {
            // Unfreeze the game
            Time.timeScale = 1f;

            // Load the main menu scene
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void OnContinueButtonClick()
    {
        // Unfreeze the game
        Time.timeScale = 1f;

        // Load the scene with the specified index
        SceneManager.LoadScene(continueSceneIndex);
    }

    public void Quit()
    {
        // Quit the application
        Application.Quit();
    }
}
