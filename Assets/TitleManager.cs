using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public AudioSource SFX;

    public void StartGame()
    {
        // Load the main game scene
        SceneManager.LoadScene("Scene 1");
        SFX.Play();
    }

    public void ExitGame()
    {
        // Quit the application (works in standalone builds)
        Application.Quit();
        SFX.Play();
    }

    public void Option()
    {
        SFX.Play();
    }

    public void ShowHelp()
    {
        // Load the main game scene
        SceneManager.LoadScene("Help");
        SFX.Play();
    }
}
