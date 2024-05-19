using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverUI; // Reference to the Game Over UI object
    public AudioSource gameOverSound; // Reference to the AudioSource for the Game Over sound
    public CoinManager coinManager; // Reference to the CoinManager script
    public TMP_Text finalScoreText; // Reference to a TextMesh Pro TMP_Text component to display the final score

    private bool isGameOver = false;

    private void Start()
    {
        gameOverUI.SetActive(false); // Ensure the game over UI is hidden initially
    }

    void Update()
    {
        // Check for game over condition
        if (!isGameOver && IsGameOverConditionMet())
        {
            ShowGameOverMenu();
        }
    }

    bool IsGameOverConditionMet()
    {
        // Implement your game over condition here
        // For example, if player's health reaches zero
        // or if time runs out, etc.
        return false; // Placeholder, replace with actual condition
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

    public void Retry()
    {
        // Unfreeze the game
        Time.timeScale = 1f;

        // Reset game state here
        ResetGameState();

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ResetGameState()
    {
        // Reset any game state here
        // For example, you can reset health, score, etc.
        // You should also reset any GameObjects or Images that need to be reset

        // Reset health controller
        HealthController healthController = FindObjectOfType<HealthController>();
        if (healthController != null)
        {
            healthController.ResetHealthState();
        }

        // Reset any other components or GameObjects as needed
    }


    public void MainMenu()
    {
        // Unfreeze the game
        Time.timeScale = 1f;

        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        // Quit the application
        Application.Quit();
    }
}
