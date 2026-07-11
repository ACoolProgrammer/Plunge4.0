using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Gravity Settings")]
    public float globalGravityModifier = 1f;
    private float gravityStrengthX = 0;
    private float gravityStrengthY = -9.81f;

    [Header("Ground Check")]
    public float checkOffsetDistance = 1.0f;
    public float checkRadius = 0.3f;
    public LayerMask whatIsGround;

    //Private

    private bool isGrounded;
    private Rigidbody2D rb;
    private float moveInputX = 0f;
    private float groundCheckDisableTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        gravityStrengthY = -9.81f * globalGravityModifier;
        UpdateGlobalGravity();
    }

    private void Update()
    {
        moveInputX = 0f;
        if (Input.GetKey(KeyCode.D))
        {
            moveInputX = 1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveInputX = -1f;
        }

        // --- Reverse Ceiling Controls ---
        if (Physics2D.gravity.y > 0.1f)
        {
            moveInputX *= -1f;
            
        }

        CheckIfGrounded();

        GravityInput();
        JumpInput();

    }

    private void FixedUpdate()
    {
        MovePlayerRelativeToGravity();
    }

    private void CheckIfGrounded()
    {

        // If the cooldown timer is active, force grounded to false and count down
        if (groundCheckDisableTimer > 0)
        {
            groundCheckDisableTimer -= Time.deltaTime;
            isGrounded = false;
            return;
        }

        Vector2 gravityDir = Physics2D.gravity.normalized;
        if (gravityDir.sqrMagnitude < 0.01f) return;

        Vector2 checkPosition = (Vector2)transform.position + (gravityDir * checkOffsetDistance);

        isGrounded = Physics2D.OverlapCircle(checkPosition, checkRadius, whatIsGround);
    }

    private void JumpInput()
    {
        // Triggers when pressing the W key
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            // Find "Up" direction (opposite of gravity vector)
            Vector2 gravityDir = Physics2D.gravity.normalized;
            Vector2 jumpDirection = -gravityDir;

            // Remove existing velocity along the gravity axis before applying jump force
            float currentFallVelocity = Vector2.Dot(rb.linearVelocity, gravityDir);
            rb.linearVelocity -= gravityDir * currentFallVelocity;

            // Apply the jump force vector
            rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void GravityInput()
{
    if (!isGrounded) return;

    float gravityMagnitude = 9.81f * globalGravityModifier;
    bool gravityChanged = false;

    if (Input.GetKeyDown(KeyCode.UpArrow)) {
        gravityStrengthX = 0;
        gravityStrengthY = gravityMagnitude;
        gravityChanged = true;
    } 
    else if (Input.GetKeyDown(KeyCode.DownArrow)) {
        gravityStrengthX = 0;
        gravityStrengthY = -gravityMagnitude;
        gravityChanged = true;
    } 
    else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
        gravityStrengthX = -gravityMagnitude;
        gravityStrengthY = 0;
        gravityChanged = true;
    } 
    else if (Input.GetKeyDown(KeyCode.RightArrow)) {
        gravityStrengthX = gravityMagnitude;
        gravityStrengthY = 0;
        gravityChanged = true;
    }

    if (gravityChanged)
    {
        UpdateGlobalGravity();
        // Disable ground checking for 0.15 seconds to let the player leave the floor
        groundCheckDisableTimer = 0.15f; 
        isGrounded = false; 
    }
}

    void UpdateGlobalGravity()
    {
        Vector2 newGravity = new Vector2(gravityStrengthX, gravityStrengthY);
        Physics2D.gravity = newGravity;

        Vector2 gravityDir = newGravity.normalized;
        if (gravityDir.sqrMagnitude < 0.01f) return;

        // Set angle based on on downward gravity
        float angle = Mathf.Atan2(gravityDir.y, gravityDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }

    private void MovePlayerRelativeToGravity()
    {
        Vector2 gravityDir = Physics2D.gravity.normalized;

        if (gravityDir.sqrMagnitude < 0.01f) return;

        // Calculate the local "Right" vector perpendicular to gravity
        Vector2 localRight = new Vector2(-gravityDir.y, gravityDir.x);

        // Project A/D input onto the local horizontal plane
        Vector2 movementDirection = localRight * moveInputX;

        // Retain existing velocity along the gravity axis (falling/jumping)
        float currentFallVelocity = Vector2.Dot(rb.linearVelocity, gravityDir);

        // Apply the combined velocities
        rb.linearVelocity = movementDirection * moveSpeed + (gravityDir * currentFallVelocity);
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 gravityDir = Physics2D.gravity.normalized;
        
        if (gravityDir.sqrMagnitude < 0.01f) gravityDir = Vector2.down;

        Vector2 checkPosition = (Vector2)transform.position + (gravityDir * checkOffsetDistance);
        UnityEngine.Gizmos.color = UnityEngine.Color.green;
        UnityEngine.Gizmos.DrawWireSphere(checkPosition, checkRadius);
    }
}