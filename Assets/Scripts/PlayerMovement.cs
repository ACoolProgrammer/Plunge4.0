using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private float gravityStrengthX = 0;
    private float gravityStrengthY = 0;
    public float globalGravityModifier = 1;
    void Start()
    {
        // Get the Rigidbody2D component attached to the player
        rb = GetComponent<Rigidbody2D>();
        globalGravityModifier = 1;
    }

    
void Update() {
    // Updates Global Gravity When Arrow Keys are pressed
    if (Input.GetKeyDown(KeyCode.UpArrow)) {
        gravityStrengthX = 0;
        gravityStrengthY = 1 * globalGravityModifier;
        UpdateGlobalGravity();
    } 
    else if (Input.GetKeyDown(KeyCode.DownArrow)) {
        gravityStrengthX = 0;
        gravityStrengthY = -1 * globalGravityModifier;
        UpdateGlobalGravity();
    } 
    else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
        gravityStrengthX = -1 * globalGravityModifier;
        gravityStrengthY = 0;
        UpdateGlobalGravity();
    } 
    else if (Input.GetKeyDown(KeyCode.RightArrow)) {
        gravityStrengthX = 1 * globalGravityModifier;
        gravityStrengthY = 0;
        UpdateGlobalGravity();
    }
}

void UpdateGlobalGravity() {
    Physics2D.gravity = new Vector2(gravityStrengthX, gravityStrengthY);
}

}
