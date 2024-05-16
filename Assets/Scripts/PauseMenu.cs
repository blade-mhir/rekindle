using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the pause menu UI GameObject
    public AudioSource PauseMenuSound; // Reference to the audio manager AudioSource

    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        // Add functionality to quit the game (optional)
        // You can use SceneManager.LoadScene or Application.Quit() here
        Debug.Log("Quitting the game...");
        Application.Quit(); // Example: Quit the application
    }

    public void MainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene("MainMenuSceneName"); // Replace with your main menu scene name
        Time.timeScale = 1f; // Ensure time is unpaused even if paused before

        // Unmute audio (optional)
        if (PauseMenuSound != null)
        {
            PauseMenuSound.mute = false;
        }
    }
}
