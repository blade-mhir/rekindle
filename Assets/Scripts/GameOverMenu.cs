using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverUI; // Reference to the Game Over UI object
    public AudioSource gameOverSound; // Reference to the AudioSource for the Game Over sound
    public CoinManager coinManager; // Reference to the CoinManager script
    public TMP_Text finalScoreText; // Reference to a TextMesh Pro TMP_Text component to display the final score
    public HealthController healthController; // Reference to the HealthController script

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
    }

    public void Restart()
    {
        if (isGameOver) // Only proceed if game over
        {
            // Unfreeze the game
            Time.timeScale = 1f;

            // Reset the health and shield state
            if (healthController != null)
            {
                healthController.ResetHealthState();
            }

            // Reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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


    public void Quit()
    {
        // Quit the application
        Application.Quit();
    }
}
