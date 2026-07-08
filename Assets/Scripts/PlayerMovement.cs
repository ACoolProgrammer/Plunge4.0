using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Gravity Settings")]
    public float globalGravityModifier = 1f;
    private float gravityStrengthX = 0;
    private float gravityStrengthY = -9.81f; // Default standard gravity

    private Rigidbody2D rb;
    private CustomInput input = new CustomInput();
    private Vector2 moveInput = Vector2.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Initialize gravity to standard downward pull
        gravityStrengthY = -9.81f * globalGravityModifier;
        UpdateGlobalGravity();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCanceled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCanceled;
    }

    private void Update() 
    {
        HandleGravityInput();
    }

    private void FixedUpdate()
    {
        MovePlayerRelativeToGravity();
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveInput = value.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        moveInput = Vector2.zero;
    }

    private void HandleGravityInput()
    {
        // Standard gravity strength baseline
        float gravityMagnitude = 9.81f * globalGravityModifier;

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            gravityStrengthX = 0;
            gravityStrengthY = gravityMagnitude;
            UpdateGlobalGravity();
        } 
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            gravityStrengthX = 0;
            gravityStrengthY = -gravityMagnitude;
            UpdateGlobalGravity();
        } 
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            gravityStrengthX = -gravityMagnitude;
            gravityStrengthY = 0;
            UpdateGlobalGravity();
        } 
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            gravityStrengthX = gravityMagnitude;
            gravityStrengthY = 0;
            UpdateGlobalGravity();
        }
    }

    void UpdateGlobalGravity() 
    {
        Physics2D.gravity = new Vector2(gravityStrengthX, gravityStrengthY);
    }

    private void MovePlayerRelativeToGravity()
    {
        // 1. Find the local "Down" direction based on current gravity vector
        Vector2 gravityDir = Physics2D.gravity.normalized;

        if (gravityDir == Vector2.zero) return;

        // 2. Calculate the local "Right" vector perpendicular to gravity
        // This ensures pressing right always moves clockwise relative to gravity
        Vector2 localRight = new Vector2(-gravityDir.y, gravityDir.x);

        // 3. Project horizontal input onto the local horizontal plane
        Vector2 movementDirection = localRight * moveInput.x;

        // 4. Retain existing velocity along the gravity axis (so falling works)
        float currentFallVelocity = Vector2.Dot(rb.linearVelocity, gravityDir);
        
        // 5. Apply the combined velocities
        rb.linearVelocity = movementDirection * moveSpeed + (gravityDir * currentFallVelocity);
    }
}