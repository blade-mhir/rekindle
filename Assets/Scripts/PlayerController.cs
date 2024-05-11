using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;

    private float minX = -6.25f;
    private float maxX = 6.92f;
    // Updated Y boundary values
    private float minY = -6.95f; 
    private float maxY = 5.93f;

    // Reference to the HealthController script
    [SerializeField] private HealthController healthController;

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
        Vector2 desiredPos = rb.position + movement * (moveSpeed * Time.fixedDeltaTime);

        // Clamp desired position within boundaries
        desiredPos.x = Mathf.Clamp(desiredPos.x, minX, maxX);
        desiredPos.y = Mathf.Clamp(desiredPos.y, minY, maxY);

        // Move the rigidbody to the clamped position
        rb.MovePosition(desiredPos);
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

    // Function called by HealthController when player takes fatal damage
    public void Die()
    {
        // Implement your player death logic here (e.g., disable movement, play death animation)
        Debug.Log("Player Died!"); // Placeholder for now

        // You can disable movement using rb.isKinematic = true;
        // Play death animation using myAnimator.SetTrigger("Die"); (assuming you have a Die trigger animation)
    }
}
