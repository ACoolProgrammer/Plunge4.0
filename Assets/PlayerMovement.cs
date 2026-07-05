using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private Vector2 moveInput;
    
    private float gravityStrengthX = 0;

    private float gravityStrengthY = 0;

    private float playerVelocity = 1;
    public float gravityModifierX;
    public float gravityModifierY;
    void Start()
    {
        // Get the Rigidbody2D component attached to the player
        rb = GetComponent<Rigidbody2D>();
    }

    
 void Update()
{
    Physics2D.gravity = new Vector2(gravityStrengthX, gravityStrengthY); 
    // Check if the Up Arrow is pressed
    if (Input.GetKeyDown(KeyCode.UpArrow))
    {
        gravityStrengthX = 0 * gravityModifierX;
        gravityStrengthY = 1 * gravityModifierY;
        //FreezeX();
    }
    if (Input.GetKeyDown(KeyCode.DownArrow))
    {
        gravityStrengthX = 0 * gravityModifierX;
        gravityStrengthY = -1 * gravityModifierY;
        //FreezeX();
    }
    if (Input.GetKeyDown(KeyCode.LeftArrow))
    {
        gravityStrengthX = -1 * gravityModifierX;
        gravityStrengthY = 0 * gravityModifierY;
        //FreezeY();
    }
    if (Input.GetKeyDown(KeyCode.RightArrow))
    {
        gravityStrengthX = 1 * gravityModifierX;
        gravityStrengthY = 0 * gravityModifierY;
        //FreezeY();
    }
}
    void FreezeX()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
    }
    void FreezeY()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
    }
}
