using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverUI; // Reference to the Game Over UI object
    public AudioSource gameOverSound; // Reference to the AudioSource for the Game Over sound

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
        return false;
    }

    void ShowGameOverMenu()
    {
        // Activate the Game Over menu UI
        gameOverUI.SetActive(true);
    }

    public void Retry()
    {
        // Unfreeze the game
        Time.timeScale = 1f;

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
}
