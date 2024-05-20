using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void StartGame()
    {
        // Load the main game scene
        SceneManager.LoadScene("Scene 1");
    }

    public void ExitGame()
    {
        // Quit the application (works in standalone builds)
        Application.Quit();
    }

    public void ShowHelp()
    {
        // Load the main game scene
        SceneManager.LoadScene("Help");
    }
}
