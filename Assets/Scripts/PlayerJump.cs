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
    public Vector2 wallJumpStrength;
    public float wallJumpRaycastLength;
    public Vector2 wallJumpRaycastOffset;

    // private variables
    new Rigidbody2D rigidbody;
    PlayerController player;

    InputAction jumpAction;

    bool jumpStarted = false;
    bool jumpCut = false;
    float jumpTime = 0;
    float currentJumpStrength;
    Vector2 gravity;

    Vector2 preJumpVelocity;
    
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
        Vector2 addedJumpVelocity = rigidbody.linearVelocity - preJumpVelocity;

        // Add jump force while jump is held and max jump time is not reached
        if (jumpStarted)
        {
            if (jumpTime < maxJumpTime + jumpHoverTime && jumpAction.IsPressed())
            {
                rigidbody.AddForce(Vector2.up * (currentJumpStrength * Mathf.Max(0, 1-jumpTime/maxJumpTime) + gravity.y * dt - addedJumpVelocity.y), ForceMode2D.Impulse);
                jumpTime += Time.fixedDeltaTime;
            }
            else
            {
                
                cancelJump();
                jumpCut = true;
                rigidbody.AddForce(Vector2.down * addedJumpVelocity.y * endJumpVelocityMultiplier, ForceMode2D.Impulse);
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
        cancelJump();

        // Normal Jump
        if (player.IsGrounded())
        {
            jumpStarted = true;
            currentJumpStrength = jumpStrength;
            return;
        }
        
        // Wall jump
        Debug.Log("JUMPEFSLFDSFJKFDJSL");
        if(Physics2D.Raycast(transform.position - (Vector3) wallJumpRaycastOffset, Vector2.left, wallJumpRaycastLength, 1 << gameObject.layer))
        {
            jumpStarted = true;
            preJumpVelocity = rigidbody.linearVelocity;
            currentJumpStrength = wallJumpStrength.y;
            rigidbody.AddForce(new Vector2(wallJumpStrength.x, 0), ForceMode2D.Impulse);
            return;
        }
        else if(Physics2D.Raycast(transform.position + (Vector3) wallJumpRaycastOffset, Vector2.right, wallJumpRaycastLength, 1 << gameObject.layer))
        {
            jumpStarted = true;
            preJumpVelocity = rigidbody.linearVelocity;
            currentJumpStrength = wallJumpStrength.y;
            rigidbody.AddForce(new Vector2(-wallJumpStrength.x, 0), ForceMode2D.Impulse);
            return;
        }




    }

    public void cancelJump(){
        jumpStarted = false;
        jumpCut = false;
        jumpTime = 0;
        preJumpVelocity = rigidbody.linearVelocity;
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
