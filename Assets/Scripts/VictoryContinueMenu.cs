using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class VictoryContinueMenu : MonoBehaviour
{
    public GameObject victoryContinueUI; // Change this to public
    public CoinManager coinManager; // Reference to the CoinManager script
    public TMP_Text totalScoreText; // Reference to a TextMesh Pro TMP_Text component to display the total score

    private void Start()
    {
        // Ensure the Victory Continue UI is properly referenced
        if (victoryContinueUI == null)
        {
            Debug.LogError("Victory Continue UI is not assigned.");
        }

        // Display the total score when the menu starts
        DisplayTotalScore();
    }

    void DisplayTotalScore()
    {
        // Get the total score from the CoinManager
        int totalScore = coinManager.coinScore;

        // Display the total score
        totalScoreText.text = "Score: " + totalScore.ToString();
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
