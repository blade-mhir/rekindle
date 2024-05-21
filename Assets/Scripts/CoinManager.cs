using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public int coinScore = 0;
    public TMP_Text coinScoreText;

    private void Start()
    {
        UpdateScoreText();
    }

    public void AddCoins(int amount)
    {
        coinScore += amount;
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        coinScoreText.text = "x " + coinScore.ToString();
    }

    public void ResetCoinScore()
    {
        coinScore = 0;
        UpdateScoreText();
    }

    // private void OnEnable()
    // {
    //     GameManager.OnGameOver += ResetCoinScore;
    // }

    // private void OnDisable()
    // {
    //     GameManager.OnGameOver -= ResetCoinScore;
    // }
}
