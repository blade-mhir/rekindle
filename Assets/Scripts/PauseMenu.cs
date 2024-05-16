using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the pause menu UI GameObject
    public AudioSource PauseMenuSound; // Reference to the audio manager AudioSource
    public GameObject gameOverMenuUI; // Reference to the Game Over menu UI GameObject

    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOverMenuUI.activeInHierarchy)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Freeze time
        pauseMenuUI.SetActive(true); // Show pause menu UI
        isPaused = true;

        // Mute audio (optional)
        if (PauseMenuSound != null)
        {
            PauseMenuSound.mute = true;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume time
        pauseMenuUI.SetActive(false); // Hide pause menu UI
        isPaused = false;

        // Unmute audio (optional)
        if (PauseMenuSound != null)
        {
            PauseMenuSound.mute = false;
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit(); // Quit the application
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenuSceneName"); // Replace with your main menu scene name
        Time.timeScale = 1f; // Ensure time is unpaused even if paused before

        // Unmute audio (optional)
        if (PauseMenuSound != null)
        {
            PauseMenuSound.mute = false;
        }
    }

    public void DisablePauseMenu()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        pauseMenuUI.SetActive(false); // Ensure pause menu is hidden
    }

    public void ActivateGameOverMenu()
    {
        DisablePauseMenu(); // Disable the pause menu
        gameOverMenuUI.SetActive(true); // Activate the Game Over menu
        Time.timeScale = 0f; // Freeze time if needed for Game Over state
    }
}
