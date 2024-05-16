using UnityEngine;
using TMPro; // Import the TextMesh Pro namespace

public class CoinManager : MonoBehaviour
{
    public int coinScore = 0;
    public TMP_Text coinScoreText; // Reference to a TextMesh Pro TMP_Text component to display the score

    private void Start()
    {
        UpdateScoreText();
    }

    public void AddCoins(int amount)
    {
        coinScore += amount;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        coinScoreText.text = "x " + coinScore.ToString();
    }
}
