using System.Collections;
using UnityEngine;

public class DashCardActivation : MonoBehaviour
{
    [SerializeField] private float dashDistance = 5f; // Distance the player dashes (customizable in inspector)
    [SerializeField] private float dashSpeed = 20f; // Speed of the player when dashing (customizable in inspector)
    [SerializeField] private float dashDuration = 0.2f; // Duration of the dash effect (customizable in inspector)
    [SerializeField] private float dashCooldown = 30f; // Cooldown period after using the Dash Card (customizable in inspector)

    private PlayerController playerController;
    private bool dashCardActive = false; // Flag to check if Dash Card is active
    private bool canDash = false; // Flag to check if the player can dash
    private float lastDashTime = -30f; // Time when the player last dashed
    private float powerUpDuration = 0f; // Duration of the active Dash Card power-up

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (dashCardActive && Input.GetKeyDown(KeyCode.E))
        {
            ActivateDashCard();
        }

        if (canDash && Input.GetMouseButtonDown(1)) // Right-click to dash
        {
            StartCoroutine(Dash());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DashCard"))
        {
            dashCardActive = true;
            Destroy(collision.gameObject); // Destroy the power-up on collision
        }
    }

    private void ActivateDashCard()
    {
        canDash = true;
        dashCardActive = false;
        powerUpDuration = dashDuration; // Set power-up duration
    }

    private IEnumerator Dash()
    {
        while (powerUpDuration > 0f) // Check if the power-up duration has not reached 0
        {
            if (Time.time >= lastDashTime + dashCooldown)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position in world space
                Vector2 dashDirection = (mousePosition - (Vector2)transform.position).normalized; // Calculate direction towards the mouse
                Vector2 originalPosition = transform.position;
                Vector2 targetPosition = originalPosition + dashDirection * dashDistance;

                float distanceRemaining = Vector2.Distance(originalPosition, targetPosition);
                float startTime = Time.time; // Record the start time of the dash

                while (distanceRemaining > 0f)
                {
                    float moveDistance = dashSpeed * Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveDistance);
                    distanceRemaining = Vector2.Distance(transform.position, targetPosition);

                    // Calculate the elapsed time since the start of the dash
                    float elapsedTime = Time.time - startTime;

                    // Calculate how much time has passed since the last iteration of the loop
                    float deltaTime = Time.deltaTime;

                    // Decrease power-up duration based on the time passed in this iteration
                    powerUpDuration -= deltaTime;

                    yield return null;
                }

                transform.position = targetPosition; // Ensure the player reaches the target position
                lastDashTime = Time.time;
            }
        }

        // Power-up duration reached 0, initiate cooldown
        StartCoroutine(DashCooldown());
    }


    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        dashCardActive = true; // Reactivate Dash Card after cooldown
    }
}
