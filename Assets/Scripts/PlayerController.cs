using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f; // Base movement speed
    [SerializeField] private float fireMoveSpeed = 2.5f; // Movement speed during Fire powerup (customizable in inspector)
    [SerializeField] private float fireDuration = 5f; // Duration of Fire powerup effect (customizable in inspector)
    [SerializeField] private float increasedFireRate = 0.5f; // Desired fire rate during Fire powerup (adjustable)
    [SerializeField] private float increasedBulletForce = 2.0f; // Desired bullet force during Fire powerup (adjustable)
    [SerializeField] private float coffeeMoveSpeed = 2f; // Movement speed during Coffee powerup
    [SerializeField] private float coffeeDuration = 5f; // Duration of Coffee powerup effect (customizable in inspector)

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;

    private float minX = -6.25f;
    private float maxX = 6.92f;
    private float minY = -6.95f;
    private float maxY = 5.93f;

    [SerializeField] private HealthController healthController;

    private bool isPowerUpActive = false; // Flag for active powerup
    private PowerUpType activePowerUp; // Type of active powerup (Coffee or Fire)
    private float powerUpStartTime; // Time when the powerup was activated

    private enum PowerUpType { None, Coffee, Fire } // Enum for powerup types

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        PlayerInput();
        UpdatePowerUp();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        // Calculate desired position based on movement and speed
        Vector2 desiredPos = rb.position + movement * (GetMovementSpeed() * Time.fixedDeltaTime);

        // Clamp desired position within boundaries
        desiredPos.x = Mathf.Clamp(desiredPos.x, minX, maxX);
        desiredPos.y = Mathf.Clamp(desiredPos.y, minY, maxY);

        // Move the rigidbody to the clamped position
        rb.MovePosition(desiredPos);
    }

    private float GetMovementSpeed()
    {
        switch (activePowerUp)
        {
            case PowerUpType.Coffee:
                return coffeeMoveSpeed;
            case PowerUpType.Fire:
                return fireMoveSpeed;
            default:
                return moveSpeed;
        }
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRender.flipX = true;
        }
        else
        {
            mySpriteRender.flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Coffee"))
        {
            ActivatePowerUp(PowerUpType.Coffee);
            Destroy(collision.gameObject); // Destroy the powerup on collision
        }
        else if (collision.gameObject.CompareTag("Fire"))
        {
            ActivatePowerUp(PowerUpType.Fire);
            Destroy(collision.gameObject); // Destroy the powerup on collision
        }
    }

    private void ActivatePowerUp(PowerUpType powerUpType)
    {
        if (!isPowerUpActive || activePowerUp != powerUpType)
        {
            isPowerUpActive = true;
            activePowerUp = powerUpType;
            powerUpStartTime = Time.time;

            if (powerUpType == PowerUpType.Fire)
            {
            Shooting shootingScript = GetComponent<Shooting>();
            if (shootingScript != null)
            {
                shootingScript.SetFirePowerUpValues(increasedFireRate, increasedBulletForce);
            }
            }
        }
    }

      public void Die()
    {
        // Implement your player death logic here
        Debug.Log("Player Died!"); // Placeholder for now
    }

    private void UpdatePowerUp()
    {
        if (isPowerUpActive && Time.time >= powerUpStartTime + GetPowerUpDuration())
        {
            DeactivatePowerUp();
        }
    }

    private float GetPowerUpDuration()
    {
        switch (activePowerUp)
        {
            case PowerUpType.Coffee:
                return coffeeDuration;
            case PowerUpType.Fire:
                return fireDuration;
            default:
                return 0f; // No duration for inactive powerup
        }
    }
    private void DeactivatePowerUp()
    {
        isPowerUpActive = false;
        activePowerUp = PowerUpType.None;

         // Reset Shooting script to default values
        Shooting shootingScript = GetComponent<Shooting>();
        if (shootingScript != null)
        {
            shootingScript.ResetFirePowerUpValues();
        }
    }
}
