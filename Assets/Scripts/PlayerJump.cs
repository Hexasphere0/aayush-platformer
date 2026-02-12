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
    public float wallJumpMovementFreezeTime;

    [Header("Coyote Time")]
    public float coyoteTime;
    public float jumpInputBufferTime;

    [Header("Clip Jump Velocity Addition")]
    public float clipJumpVelocityAdditionTime;
    public float verticalPostClipVelocityMultiplier;
    public float horizontalPostClipVelocityMultiplier;

    // private variables
    new Rigidbody2D rigidbody;
    PlayerController player;
    InputAction jumpAction;
    Vector2 gravity;

    // Jump state variables
    bool jumpStarted = false;
    bool jumpCut = false;
    float jumpTime = 0;
    float currentJumpStrength;
    
    // Coyote time variables
    float timeSinceCanJump = 0;
    JumpType priorJumpType = JumpType.None; // 0 = regular, 1 = wall jump from left, 2 = wall jump from right
    float timeSinceJumpInput = 0;

    // Neutral Jump Prevention
    float wallJumpXCoordinate = 0;
    JumpType lastExecutedJumpType = JumpType.None;

    // Clip Jump Variables
    float timeSinceClip = 0;



    public static PlayerJump instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple PlayerJumps!");
        }
    }
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        player = GetComponent<PlayerController>();

        jumpAction = InputSystem.actions.FindAction("Jump");
        jumpAction.performed += DetectJumpInput;

        gravity = player.gravity;
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        // Start a jump if jump input is detected within coyote time

        timeSinceCanJump += dt;
        timeSinceJumpInput += dt;
        timeSinceClip += dt;

        if (!jumpStarted)
        {

            JumpType jumpType = canJump();

            if(jumpType != JumpType.None)
            {
                timeSinceCanJump = 0;
                priorJumpType = jumpType;
            }

            if(jumpType == JumpType.Regular){
                // Reset wall jump state variables when landing on the ground to allow wall jumps again
                lastExecutedJumpType = JumpType.None;
                wallJumpXCoordinate = -1000000000;
            }

            if (timeSinceCanJump <= coyoteTime && timeSinceJumpInput <= jumpInputBufferTime && priorJumpType != JumpType.None)
            {
                
                if(!(priorJumpType != JumpType.Regular && lastExecutedJumpType == priorJumpType && Mathf.Abs(rigidbody.position.x - wallJumpXCoordinate) < wallJumpRaycastLength + 0.3f))
                {
                    // Prevent repeated jumps on the same wall by checking if the player is trying to wall jump in the same direction within a short distance from the last wall jump
 
                    StartJump(priorJumpType);
                    priorJumpType = JumpType.None;
                    timeSinceJumpInput += jumpInputBufferTime + 1; // reset jump input timer so that buffered jump input is not used for multiple jumps
                }

            }
        }

        // Add jump force while jump is held and max jump time is not reached
        if (jumpStarted)
        {
            if (jumpTime < maxJumpTime + jumpHoverTime && jumpAction.IsPressed())
            {
                rigidbody.AddForce(Vector2.up * (currentJumpStrength * Mathf.Max(0, 1-jumpTime/maxJumpTime) + gravity.y * dt - rigidbody.linearVelocity.y), ForceMode2D.Impulse);
                jumpTime += Time.fixedDeltaTime;
            }
            else
            {
                
                CancelJump();
                jumpCut = true;
                rigidbody.AddForce(Vector2.down * rigidbody.linearVelocity.y * endJumpVelocityMultiplier, ForceMode2D.Impulse);
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

    void DetectJumpInput(InputAction.CallbackContext context)
    {   

        timeSinceJumpInput = 0;

    }

    void StartJump(JumpType jumpType)
    {
        // We cancel a jump before starting a new one to reset all jump state variables.
        CancelJump();



        lastExecutedJumpType = jumpType;
        // Set jump state variables and reset vertical velocity to 0 to ensure consistent jump height regardless of current vertical velocity
        jumpStarted = true;

        if(rigidbody.linearVelocity.y < 0){
            rigidbody.AddForce(new Vector2(0, -rigidbody.linearVelocity.y), ForceMode2D.Impulse);
        }
        
        // Normal Jump
        if (jumpType == JumpType.Regular)
        {
            currentJumpStrength = jumpStrength;
            return;
        }
        
        // Wall jump

        currentJumpStrength = wallJumpStrength.y;

        // Apply horizontal wall jump force
    

        wallJumpXCoordinate = rigidbody.position.x;


        if(jumpType == JumpType.WallLeft)
        {
            rigidbody.AddForce(new Vector2(wallJumpStrength.x, 0), ForceMode2D.Impulse);
            return;
        }
        else if(jumpType == JumpType.WallRight)
        {
            rigidbody.AddForce(new Vector2(-wallJumpStrength.x, 0), ForceMode2D.Impulse);
            return;
        }
    }

    JumpType canJump()
    {
        if(player.IsGrounded())
        {
            return JumpType.Regular;
        }
        else if(Physics2D.Raycast(transform.position - (Vector3) wallJumpRaycastOffset, Vector2.left, wallJumpRaycastLength, 1 << gameObject.layer))
        {
            return JumpType.WallLeft;
        }
        else if(Physics2D.Raycast(transform.position + (Vector3) wallJumpRaycastOffset, Vector2.right, wallJumpRaycastLength, 1 << gameObject.layer))
        {
            return JumpType.WallRight;
        }
        else
        {
            return JumpType.None;
        }
    }


    public void CancelJump(){
        jumpStarted = false;
        jumpCut = false;
        jumpTime = 0;
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

enum JumpType
{
    Regular,
    WallLeft,
    WallRight,
    None = -1
}
