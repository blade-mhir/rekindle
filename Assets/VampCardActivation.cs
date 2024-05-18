using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampCardActivation : MonoBehaviour
{
    public static bool isVampCardActive = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.CompareTag("VampCard"))
        {
            if (CardCollectionManager.Instance.CanCollectCard("VampCard"))
            {
                isVampCardActive = true;
                CardCollectionManager.Instance.CollectCard("VampCard");
                Destroy(collision.gameObject); // Remove the Vampirism Card from the scene
            }
        }
    }
}

