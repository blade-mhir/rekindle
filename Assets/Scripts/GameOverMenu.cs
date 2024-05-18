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

    // Update is called once per frame
    void Update()
    {
        // Check for game over condition
        if (!isGameOver && IsGameOverConditionMet())
        {
            // Freeze the game
            Time.timeScale = 0f;
            isGameOver = true;

            // Show Game Over menu
            ShowGameOverMenu();

            // Display final score
            DisplayFinalScore();

            // Play Game Over sound
            if (gameOverSound != null)
            {
                gameOverSound.Play();
            }
        }
    }

    bool IsGameOverConditionMet()
    {
        // Implement your game over condition here
        // For example, if player's health reaches zero
        // or if time runs out, etc.
        return false; // Placeholder, replace with actual condition
    }

    void ShowGameOverMenu()
    {
        // Activate the Game Over menu UI
        gameOverUI.SetActive(true);
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

        // Reset player state
        ResetPlayerState();

        // Reload Scene 1
        SceneManager.LoadScene("Scene 1");
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

    private void ResetPlayerState()
    {
        // Find the player object and reset its state
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.ResetState(); // Add a method in PlayerController to reset its state
        }
    }
}
