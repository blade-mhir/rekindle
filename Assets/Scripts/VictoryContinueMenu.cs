using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryContinueMenu : MonoBehaviour
{
    public GameObject victoryContinueUI; // Change this to public

    private void Start()
    {
        // Ensure the Victory Continue UI is properly referenced
        if (victoryContinueUI == null)
        {
            Debug.LogError("Victory Continue UI is not assigned.");
        }
    }

    public void OnContinueButtonClick()
    {
        // Load Scene 2
        SceneManager.LoadScene(2);
    }

    public void OnMainMenuButtonClick()
    {
        // Load Main Menu Scene (assuming it's Scene 0)
        SceneManager.LoadScene(0);
    }
}
