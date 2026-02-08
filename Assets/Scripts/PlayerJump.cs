using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpStrength;
    public float jumpHoverTime;
    public float endJumpGravityMultiplier;
    public float endJumpVelocityMultiplier;
    public float maxJumpTime;
 
    [Header("Wall Jump")]
    public Vector2 wallJumpForce;
    public float wallJumpRaycastLength;
    public float wallJumpMovementFreezeTime;
    public Vector2 wallJumpRaycastOffset;

    // private variables
    new Rigidbody2D rigidbody;
    PlayerController player;

    InputAction jumpAction;

    bool jumpStarted = false;
    bool jumpCut = false;
    float jumpTime = 0;
    Vector2 gravity;

    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        player = GetComponent<PlayerController>();

        jumpAction = InputSystem.actions.FindAction("Jump");
        jumpAction.performed += StartJump;

        gravity = player.gravity;
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        // Add jump force while jump is held and max jump time is not reached
        if (jumpStarted)
        {
            if (jumpTime < maxJumpTime + jumpHoverTime && jumpAction.IsPressed())
            {
                rigidbody.AddForce(Vector2.up * (jumpStrength * Mathf.Max(0, 1-jumpTime/maxJumpTime) + gravity.y * dt - rigidbody.linearVelocityY), ForceMode2D.Impulse);
                jumpTime += Time.fixedDeltaTime;
            }
            else
            {
                
                jumpStarted = false;
                jumpCut = true;
                rigidbody.AddForce(Vector2.down * rigidbody.linearVelocityY * endJumpVelocityMultiplier, ForceMode2D.Impulse);
                jumpTime = 0;
            }
        }

        // Apply gravity after a jump
        if (jumpCut)
        {
            // Apply gravity multiplier
            rigidbody.AddForce(gravity * (endJumpGravityMultiplier-1), ForceMode2D.Force);
            jumpCut = false;
        }
    }

    void StartJump(InputAction.CallbackContext context)
    {   
        // Wall jump
        
        if(Physics2D.Raycast(transform.position - (Vector3) wallJumpRaycastOffset, Vector2.left, wallJumpRaycastLength, 1 << gameObject.layer))
        {
            PerformWallJump(1);
            return;
        }
        else if(Physics2D.Raycast(transform.position + (Vector3) wallJumpRaycastOffset, Vector2.right, wallJumpRaycastLength, 1 << gameObject.layer))
        {
            PerformWallJump(-1);
            return;
        }



        // Normal Jump
        if (player.IsGrounded())
        {
            jumpStarted = true;
        }
    }

    void PerformWallJump(int direction)
    {
        Debug.Log("WALLJUMP!");

        Vector2 force = new Vector2(wallJumpForce.x * direction, wallJumpForce.y);

        rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    public bool CanJumpHang ()
    {
        return jumpAction.IsPressed() && Mathf.Abs(rigidbody.linearVelocityY) <= jumpHoverTime;
    }

    public bool IsJumping()
    {
        return jumpAction.IsPressed();
    }
}
