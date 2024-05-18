using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void GameOverAction();
    public static event GameOverAction OnGameOver;

    // Method to call when the game is over
    public static void GameOver()
    {
        OnGameOver?.Invoke();
    }
}
