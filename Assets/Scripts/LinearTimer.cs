using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LinearTimer : MonoBehaviour
{
    [SerializeField] private Image timerImage; // Assign the time image in the Inspector
    [SerializeField] private GameObject gameOverText; // Assign the game over text holder in the Inspector
    [SerializeField] private float maxTime = 60f; // Initial time value (adjustable in the Inspector)

    private float currentTime;

    void Start()
    {
        currentTime = maxTime;
    }

    void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            Time.timeScale = 0f; // Pause the game on timer end (optional)
            gameOverText.gameObject.SetActive(true); // Show game over text
        }

        // Update timer image fill amount (adjust based on your image type)
        timerImage.fillAmount = currentTime / maxTime;
    }
}
