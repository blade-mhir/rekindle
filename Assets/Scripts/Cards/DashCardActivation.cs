using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashCardActivation : MonoBehaviour
{
    [SerializeField] private float dashDistance = 5f; // Distance the player dashes (customizable in inspector)
    [SerializeField] private float dashSpeed = 20f; // Speed of the player when dashing (customizable in inspector)
    [SerializeField] private float dashDuration = 0.2f; // Duration of the dash effect (customizable in inspector)
    [SerializeField] private float dashCooldown = 30f; // Cooldown period after using the Dash Card (customizable in inspector)
    [SerializeField] private GameObject dashPowerUpObject; // Reference to the dash power-up object

    [SerializeField] private Image dashCooldownFillImage; // Reference to the fill image for cooldown
    private PlayerController playerController;
    private bool canDash = false; // Flag to check if the player can dash
    private float lastDashTime = -30f; // Time when the player last dashed
    private bool isDashing = false; // Flag to check if the player is currently dashing

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Check for right mouse button input to trigger dash
        if (canDash && Input.GetMouseButtonDown(1) && !isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }

        // Activate dash card when E key is pressed and the effect is not already active and not on cooldown
        if (Input.GetKeyDown(KeyCode.E) && CardManager.instance.IsDashCardActivated())
        {
            print("activating dash");
            ActivateDashCard();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DashCard"))
        {
            print(CardManager.instance.IsInviCardActivated());
            if (!CardManager.instance.IsInviCardActivated() && !CardManager.instance.IsLaserCardActivated())
            {
                CardManager.instance.ActivateDashCard();
                ActivateDashCard();
                collision.gameObject.SetActive(false);
            }
            // Destroy(collision.gameObject); // Destroy the power-up on collision
        }
    }

    private void ActivateDashCard()
    {
        canDash = true;

        // Enable the dash power-up object
        if (dashPowerUpObject != null)
        {
            dashPowerUpObject.SetActive(true);
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position in world space
        Vector2 dashDirection = (mousePosition - (Vector2)transform.position).normalized; // Calculate direction towards the mouse
        Vector2 originalPosition = transform.position;
        Vector2 targetPosition = originalPosition + dashDirection * dashDistance;

        float distanceRemaining = Vector2.Distance(originalPosition, targetPosition);
        float elapsedTime = 0f;
        while (distanceRemaining > 0f && elapsedTime < dashDuration)
        {
            float moveDistance = dashSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveDistance);
            distanceRemaining = Vector2.Distance(transform.position, targetPosition);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Ensure the player reaches the target position
        lastDashTime = Time.time;

        // Dash completed, initiate cooldown
        canDash = false;
        StartCoroutine(DashCooldown());

        isDashing = false;
    }

    private IEnumerator DashCooldown()
    {
        // Activate cooldown fill image
        if (dashCooldownFillImage != null)
        {
            dashCooldownFillImage.gameObject.SetActive(true);
            float fillAmount = 1f; // Start with full fill
            float fillChangeRate = 1f / dashCooldown; // Rate at which fill decreases per second

            while (fillAmount > 0f)
            {
                fillAmount -= fillChangeRate * Time.deltaTime;
                dashCooldownFillImage.fillAmount = fillAmount;
                yield return null; // Wait for the next frame
            }

            // Cooldown finished, deactivate cooldown fill image
            dashCooldownFillImage.gameObject.SetActive(false);
        }

        // Cooldown finished, allow activation again
        canDash = true;
    }
}