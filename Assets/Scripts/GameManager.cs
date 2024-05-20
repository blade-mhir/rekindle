using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static event System.Action OnGameRestart; // Event to notify game restart

    private HealthController healthController;
    private GameOverMenu gameOverMenu;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        AssignReferences();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignReferences();
    }

    private void AssignReferences()
    {
        healthController = FindObjectOfType<HealthController>();
        gameOverMenu = FindObjectOfType<GameOverMenu>();

        if (healthController != null && gameOverMenu != null)
        {
            healthController.InitializeReferences(gameOverMenu);
        }
    }

    // Method to restart the game
    public void RestartGame()
    {
        // Reload the active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Notify subscribers that the game has restarted
        OnGameRestart?.Invoke();
    }
}
