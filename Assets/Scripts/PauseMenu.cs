using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the pause menu UI GameObject
    public AudioSource audioManager; // Reference to the audio manager AudioSource

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
        if (audioManager != null)
        {
            audioManager.mute = true;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume time
        pauseMenuUI.SetActive(false); // Hide pause menu UI
        isPaused = false;

        // Unmute audio (optional)
        if (audioManager != null)
        {
            audioManager.mute = false;
        }
    }

    public void QuitGame()
    {
        // Add functionality to quit the game (optional)
        // You can use SceneManager.LoadScene or Application.Quit() here
        Debug.Log("Quitting the game...");
        Application.Quit(); // Example: Quit the application
    }
}
