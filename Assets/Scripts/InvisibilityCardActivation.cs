using System.Collections;
using UnityEngine;

public class InvisibilityCardActivation : MonoBehaviour
{
    [SerializeField] private float invisibilityDuration = 10f; // Duration of invisibility power-up
    [SerializeField] private float cooldownDuration = 30f; // Cooldown duration before the power-up can be used again
    private bool canUsePowerUp = true; // Flag to track if the power-up can be used
    private bool isInvisibilityActive = false; // Flag to track if invisibility is active
    private PlayerController playerController; // Reference to the player controller
    private float invisibilityStartTime; // Time when invisibility power-up was activated

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("InviCard"))
        {
            ActivateInvisibility();
            Destroy(collision.gameObject); // Destroy the power-up on collision
        }
    }

    private void Update()
    {
        // Check if player wants to activate invisibility power-up
        if (Input.GetKeyDown(KeyCode.E) && canUsePowerUp && !isInvisibilityActive)
        {
            ActivateInvisibility();
        }

        // Check if invisibility duration has passed
        if (isInvisibilityActive && Time.time >= invisibilityStartTime + invisibilityDuration)
        {
            DeactivateInvisibility();
        }
    }

    private void ActivateInvisibility()
    {
        if (canUsePowerUp)
        {
            isInvisibilityActive = true;
            invisibilityStartTime = Time.time;
            playerController.SetInvisibility(true); // Set player invisible to enemies

            // Start cooldown timer
            StartCoroutine(CooldownTimer());
        }
    }

    private void DeactivateInvisibility()
    {
        isInvisibilityActive = false;
        playerController.SetInvisibility(false); // Set player visible to enemies
    }

    private IEnumerator CooldownTimer()
    {
        canUsePowerUp = false;
        yield return new WaitForSeconds(cooldownDuration);
        canUsePowerUp = true;
    }
}
