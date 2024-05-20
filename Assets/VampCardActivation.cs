using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampCardActivation : MonoBehaviour
{
    public static bool isVampCardActive = false;
    [SerializeField] private GameObject vampPowerUpObject; // Reference to the vamp power-up object

    // private void OnEnable()
    // {
    //     GameManager.OnGameOver += ResetVampCardState;
    // }

    // private void OnDisable()
    // {
    //     GameManager.OnGameOver -= ResetVampCardState;
    // }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("VampCard"))
        {
            if (CardCollectionManager.Instance.CanCollectCard("VampCard"))
            {
                isVampCardActive = true;
                CardCollectionManager.Instance.CollectCard("VampCard");
                Destroy(collision.gameObject); // Remove the Vampirism Card from the scene

                // Enable the vamp power-up object
                if (vampPowerUpObject != null)
                {
                    vampPowerUpObject.SetActive(true);
                }
            }
        }
    }

    // public void ResetVampCardState()
    // {
    //     // Reset vamp card activation state and related properties
    //     isVampCardActive = false;

    //     // Disable vamp power-up object
    //     if (vampPowerUpObject != null)
    //     {
    //         vampPowerUpObject.SetActive(false);
    //     }
    // }
}
