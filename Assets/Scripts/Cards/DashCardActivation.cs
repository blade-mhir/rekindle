using System.Collections;
using UnityEngine;

public class DashCardActivation : MonoBehaviour
{
    [SerializeField] private float dashDistance = 5f; // Distance the player dashes (customizable in inspector)
    [SerializeField] private float dashSpeed = 20f; // Speed of the player when dashing (customizable in inspector)
    [SerializeField] private float dashDuration = 0.2f; // Duration of the dash effect (customizable in inspector)
    [SerializeField] private float dashCooldown = 30f; // Cooldown period after using the Dash Card (customizable in inspector)
    [SerializeField] private GameObject dashPowerUpObject; // Reference to the dash power-up object
    [SerializeField] private GameObject cooldownObject; // Reference to the cooldown indicator object

    private PlayerController playerController;
    private bool canDash = false; // Flag to check if the player can dash
    private float lastDashTime = -30f; // Time when the player last dashed
    private bool isDashing = false; // Flag to check if the player is currently dashing

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        GameManager.OnGameOver += ResetDashState;
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= ResetDashState;
    }

    private void Update()
    {
        if (canDash && Input.GetMouseButtonDown(1) && !isDashing && Time.time >= lastDashTime + dashCooldown) // Right-click to dash
        {
            StartCoroutine(Dash());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!CardManager.instance.IsInviCardActivated() && !CardManager.instance.IsLaserCardActivated() && collision.gameObject.CompareTag("DashCard"))
        {
            CardManager.instance.ActivateDashCard();
            ActivateDashCard();
            Destroy(collision.gameObject); // Destroy the power-up on collision
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

        // Disable the dash power-up object
        if (dashPowerUpObject != null)
        {
            dashPowerUpObject.SetActive(false);
        }

        isDashing = false;
    }

    private IEnumerator DashCooldown()
    {
        // Enable the cooldown indicator object
        if (cooldownObject != null)
        {
            cooldownObject.SetActive(true);
        }

        yield return new WaitForSeconds(dashCooldown);
        canDash = true; // Reactivate Dash after cooldown

        // Disable the cooldown indicator object
        if (cooldownObject != null)
        {
            cooldownObject.SetActive(false);
        }
    }

    public void ResetDashState()
    {
        // Reset dash-related states and properties to their initial values
        canDash = false;
        lastDashTime = -30f;
        isDashing = false;

        // Disable dash power-up object
        if (dashPowerUpObject != null)
        {
            dashPowerUpObject.SetActive(false);
        }

        // Disable cooldown indicator object
        if (cooldownObject != null)
        {
            cooldownObject.SetActive(false);
        }
    }
}
