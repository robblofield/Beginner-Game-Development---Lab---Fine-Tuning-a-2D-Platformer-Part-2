using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement settings for different surfaces
    [Header("Movement Settings")]
    public float normalSpeed = 5f;  // Default movement speed
    public float slipperySpeed = 8f; // Increased speed on slippery surfaces
    public float stickySpeed = 3f;   // Decreased speed on sticky surfaces
    public float acceleration = 10f; // Rate at which the player accelerates
    public float deceleration = 10f; // Rate at which the player slows down
    public float jumpForce = 10f;    // Default jump force
    public float stickyJumpForce = 7f; // Reduced jump force on sticky surfaces
    public float slipperyJumpForce = 12f; // Increased jump force on slippery surfaces

    // References and state tracking
    [Header("References")]
    private Rigidbody2D rb;  // Reference to the Rigidbody2D component
    private string currentSurface = "Normal"; // Tracks the type of surface player is on
    private float currentSpeed;   // Current speed based on surface
    private float currentJumpForce; // Current jump force based on surface
    private bool isGrounded; // Checks if the player is on the ground
    private float moveInput;  // Stores horizontal movement input

    void Awake()
    {
        // Get the Rigidbody2D component attached to the player
        rb = GetComponent<Rigidbody2D>();
        // Initialize movement and jump force with default values
        currentSpeed = normalSpeed;
        currentJumpForce = jumpForce;
    }

    void Update()
    {
        HandleInput(); // Process player input
        HandleJump();  // Process jumping mechanics
    }

    void FixedUpdate()
    {
        HandleMovement(); // Handle physics-based movement
    }

    void HandleInput()
    {
        // Get horizontal movement input (-1, 0, or 1)
        moveInput = Input.GetAxisRaw("Horizontal");
    }

    void HandleMovement()
    {
        // Calculate target speed based on input and current surface speed
        float targetSpeed = moveInput * currentSpeed;
        float speedDifference = targetSpeed - rb.velocity.x;
        // Determine if acceleration or deceleration should be applied
        float accelerationRate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;
        float movementForce = speedDifference * accelerationRate;
        
        // Apply force to move the player
        rb.AddForce(new Vector2(movementForce, 0), ForceMode2D.Force);
    }

    void HandleJump()
    {
        // Check if the jump key is pressed and the player is grounded
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Apply jump force
            rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is touching different surfaces and update state
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            SetSurfaceType("Normal");
        }
        else if (collision.gameObject.CompareTag("Slippery"))
        {
            isGrounded = true;
            SetSurfaceType("Slippery");
        }
        else if (collision.gameObject.CompareTag("Sticky"))
        {
            isGrounded = true;
            SetSurfaceType("Sticky");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // When the player leaves the ground, update grounded status
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Slippery") || collision.gameObject.CompareTag("Sticky"))
        {
            isGrounded = false;
        }
    }

    private void SetSurfaceType(string surface)
    {
        // Update the current surface type
        currentSurface = surface;
        switch (surface)
        {
            case "Slippery":
                currentSpeed = slipperySpeed;
                currentJumpForce = slipperyJumpForce;
                break;
            case "Sticky":
                currentSpeed = stickySpeed;
                currentJumpForce = stickyJumpForce;
                break;
            default:
                currentSpeed = normalSpeed;
                currentJumpForce = jumpForce;
                break;
        }
        // Debug message to confirm the surface type change
        Debug.Log("Current Surface: " + currentSurface);
    }
}
