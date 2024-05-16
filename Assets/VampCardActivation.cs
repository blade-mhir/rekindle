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
            isVampCardActive = true;
            Destroy(collision.gameObject); // Remove the Vampirism Card from the scene
        }
    }
}
