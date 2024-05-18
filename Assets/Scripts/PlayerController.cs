using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f; // Base movement speed
    [SerializeField] private float fireMoveSpeed = 2.5f; // Movement speed during Fire powerup (customizable in inspector)
    [SerializeField] private float fireDuration = 5f; // Duration of Fire powerup effect (customizable in inspector)
    [SerializeField] private float increasedFireRate = 0.5f; // Desired fire rate during Fire powerup (adjustable)
    [SerializeField] private float increasedBulletForce = 2.0f; // Desired bullet force during Fire powerup (adjustable)
    [SerializeField] private GameObject firePowerUpObject;
    [SerializeField] private float coffeeMoveSpeed = 2f; // Movement speed during Coffee powerup
    [SerializeField] private float coffeeDuration = 5f; // Duration of Coffee powerup effect (customizable in inspector)
    [SerializeField] private GameObject coffeePowerUpObject;
    [SerializeField] private float sipUpCardMoveSpeed = 1.5f; // Movement speed during SipUpCard powerup
    [SerializeField] private CoinManager coinManager; // Reference to the Coin Manager
    [SerializeField] private GameObject sipCardPowerUpObject;

    private bool isInvisible = false;
    private float inviDuration = 5f; // Duration of Invisibility powerup (customizable in inspector)
    private float inviStartTime; // Time when the invisibility was activated

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;

    private float minX = -6.25f;
    private float maxX = 6.92f;
    private float minY = -7.11f;
    private float maxY = 5.93f;
    private bool isPowerUpActive = false; // Flag for active powerup
    private PowerUpType activePowerUp; // Type of active powerup (Coffee or Fire)
    private float powerUpStartTime; // Time when the powerup was activated
    private bool sipUpCardActive = false; // Flag to check if SipUpCard is active

    private enum PowerUpType { None, Coffee, Fire, SipUpCard } // Enum for powerup types

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
        GameManager.OnGameOver += ResetPlayerState; // Subscribe to the GameManager's OnGameOver event
    }

    private void OnDisable()
    {
        playerControls.Disable();
        GameManager.OnGameOver -= ResetPlayerState; // Unsubscribe from the GameManager's OnGameOver event
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
        if (isPowerUpActive)
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
        else if (sipUpCardActive)
        {
            return sipUpCardMoveSpeed;
        }
        else
        {
            return moveSpeed;
        }
    }

    public Vector2 GetMovementDirection()
    {
        return movement;
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
        if (CardCollectionManager.Instance.IsCardLimitReached())
        {
            // Optionally: Add logic to disable collision with specific tags or give feedback
            return;
        }

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
        else if (collision.gameObject.CompareTag("SipCard"))
        {
            if (CardCollectionManager.Instance.CanCollectCard("SipCard"))
            {
                ActivateSipUpCard();
                CardCollectionManager.Instance.CollectCard("SipCard");
                Destroy(collision.gameObject); // Destroy the SipCard object
            }
        }
        else if (collision.gameObject.CompareTag("InviCard"))
        {
            if (!CardManager.instance.IsDashCardActivated() && !CardManager.instance.IsLaserCardActivated())
            {
                ActivateInvisibility();
                CardManager.instance.ActivateInviCard();
                Destroy(collision.gameObject); // Destroy the InviCard object
            }
        }
        else if (collision.gameObject.CompareTag("Coin"))
        {
            CollectCoin(collision.gameObject);
            Destroy(collision.gameObject); // Destroy the powerup on collision
        }
    }

    private void ActivatePowerUp(PowerUpType powerUpType)
    {
        if (isPowerUpActive && activePowerUp != powerUpType)
        {
            DeactivatePowerUp(); // Deactivate previous power-up if different
        }

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

        // Enable corresponding power-up object
        switch (powerUpType)
        {
            case PowerUpType.Coffee:
                if (coffeePowerUpObject != null)
                {
                    coffeePowerUpObject.SetActive(true);
                }
                if (firePowerUpObject != null)
                {
                    firePowerUpObject.SetActive(false); // Deactivate Fire power-up object
                }
                break;
            case PowerUpType.Fire:
                if (firePowerUpObject != null)
                {
                    firePowerUpObject.SetActive(true);
                }
                if (coffeePowerUpObject != null)
                {
                    coffeePowerUpObject.SetActive(false); // Deactivate Coffee power-up object
                }
                break;
        }
    }

    private void ActivateInvisibility()
    {
        isInvisible = true;
        inviStartTime = Time.time;
        StartCoroutine(InvisibilityTimer());
    }

    private IEnumerator InvisibilityTimer()
    {
        yield return new WaitForSeconds(inviDuration);
        isInvisible = false;
        CardManager.instance.DeactivateAllCards();
    }

    public void SetInvisible(bool invisible)
    {
        isInvisible = invisible;
    }

    public bool IsInvisible()
    {
        return isInvisible;
    }

    private void ActivateSipUpCard()
    {
        sipCardPowerUpObject.SetActive(true);
        sipUpCardActive = true;
        moveSpeed = sipUpCardMoveSpeed;
    }

    private void CollectCoin(GameObject coin)
    {
        coinManager.AddCoins(10); // Add 10 points for each coin
        Destroy(coin); // Destroy the coin object
    }

    public void Die()
    {
        // Implement your player death logic here
        Debug.Log("Player Died!"); // Placeholder for now
        ResetPlayerState();
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
        if (shootingScript  != null)
        {
            shootingScript.ResetFirePowerUpValues();
        }

        // Disable all power-up objects
        if (coffeePowerUpObject != null)
        {
            coffeePowerUpObject.SetActive(false);
        }
        if (firePowerUpObject != null)
        {
            firePowerUpObject.SetActive(false);
        }

        // If SipUpCard is active, keep its speed
        if (sipUpCardActive)
        {
            moveSpeed = sipUpCardMoveSpeed;
        }
    }

    private void ResetPlayerState()
    {
        // Reset all player states and properties to their initial values
        isInvisible = false;
        isPowerUpActive = false;
        activePowerUp = PowerUpType.None;
        sipUpCardActive = false;

        // Deactivate all power-up objects
        if (coffeePowerUpObject != null)
        {
            coffeePowerUpObject.SetActive(false);
        }
        if (firePowerUpObject != null)
        {
            firePowerUpObject.SetActive(false);
        }
        if (sipCardPowerUpObject != null)
        {
            sipCardPowerUpObject.SetActive(false);
        }

        // Reset movement speed
        moveSpeed = 1f;

        // Reset shooting script values
        Shooting shootingScript = GetComponent<Shooting>();
        if (shootingScript != null)
        {
            shootingScript.ResetFirePowerUpValues();
        }

        // Reset animator parameters
        myAnimator.SetFloat("moveX", 0);
        myAnimator.SetFloat("moveY", 0);
    }
}

