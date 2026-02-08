using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed;

    [Header("Jump")]
    public float jumpStrength;
    public float jumpHangVelocityTreshold;
    public float groundedRaycastLength;
    public Vector2 groundedRaycastOffset;

    [Header("Wall Jump")]
    public Vector2 wallJumpForce;
    public float wallJumpRaycastLength;
    public float wallJumpMovementFreezeTime;
    public Vector2 wallJumpRaycastOffset;

    [Header("Dash")] // Broken needs to be fixed
    public float sprintStrength;
    public float sprintCooldown;

    [Header("Gravity")]
    public float hardFallGravityScale;
    public float jumpCutGravityScale;
    public float fallingGravityScale;
    public float jumpHangGravityScale;
    public float defaultGravityScale;

    [Header("Colors")]
    public Color redColor;
    public Color blueColor;

    [Header("Clamps")]
    public float maxFallSpeed;
    public float maxMovementSpeed;

    [Header("Depricated")]
    public float groundedGraceDistance;

    // Private Variables
    private bool movementFrozen { get; set; } = false;

    new Rigidbody2D rigidbody;
    SpriteRenderer spriteRenderer;

    InputAction moveAction;
    InputAction jumpAction;

    bool isJumpCut;

    float sprintTimer = 0;
    float layerChangeCooldownTime = 0f;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        
        jumpAction.performed += PerformJump;
        InputSystem.actions.FindAction("Sprint").performed += PerformSprint;

        SetGravityScale(defaultGravityScale);
    }

    void Update()
    {
        // Debug.DrawRay(transform.position + (Vector3) wallJumpRaycastOffset, Vector2.right, Color.red);

        if (jumpAction.WasReleasedThisFrame())
        {
            isJumpCut = true;
        }
        
        layerChangeCooldownTime += Time.deltaTime;
        // Swap Physics Layer *TEMPORARY!!*
        if (Keyboard.current.leftShiftKey.isPressed && layerChangeCooldownTime > 0.4f)
        {
            layerChangeCooldownTime = 0f;
            SwapPhysicsLayer();
        }

        // Color change based on layer
        if(gameObject.layer == 6)
        {
            spriteRenderer.color = redColor;
        }
        else if(gameObject.layer == 7)
        {
            spriteRenderer.color = blueColor;
        }

    }

    void FixedUpdate()
    {
        sprintTimer += Time.fixedDeltaTime;

        // Movement
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        if(!movementFrozen){
            Vector2 moveVector = new Vector2(moveValue.x, 0);

            rigidbody.AddForce(moveVector, ForceMode2D.Force);
        }

        // Gravity
        if(rigidbody.linearVelocityY < 0 && moveValue.y < 0)
        {
            SetGravityScale(hardFallGravityScale);
        }
        // Half the players vertical velocity on releasing the jump button, allows for more precise jumps
        else if (isJumpCut)
        {
            SetGravityScale(jumpCutGravityScale);
        }
        // Reduce the gravity at the top of the jump, allows for more air time
        else if(jumpAction.IsPressed() && Mathf.Abs(rigidbody.linearVelocityY) <= jumpHangVelocityTreshold)
        {
            SetGravityScale(jumpHangGravityScale);
        }
        // Make the player fall faster
        else if(rigidbody.linearVelocityY < 0)
        {
            SetGravityScale(fallingGravityScale);
        }
        else
        {
            SetGravityScale(defaultGravityScale);
        }
        
        // Clamp Fall Speed
        rigidbody.linearVelocityY = Mathf.Max(rigidbody.linearVelocityY, -maxFallSpeed);

        // Clamp Movement Speed
        int direction = rigidbody.linearVelocityX > 0 ? 1 : -1;

        rigidbody.linearVelocityX = direction * Mathf.Min(Mathf.Abs(rigidbody.linearVelocityX), maxMovementSpeed);

        // Reset jump cut
        if (IsGrounded()){
            isJumpCut = false;
        }
    }

    void SetGravityScale(float gravityScale)
    {
        rigidbody.gravityScale = gravityScale;
    }

    void PerformJump(InputAction.CallbackContext context)
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
        if (IsGrounded())
        {
            Debug.Log("JUMPING");
            Vector2 jumpVector = Vector2.up * jumpStrength;
            FreezeMovement(wallJumpMovementFreezeTime);

            rigidbody.AddForce(jumpVector); //, ForceMode2D.Impulse);
        }

    }

    void PerformSprint(InputAction.CallbackContext context)
    {
        if(sprintTimer > sprintCooldown)
        {
            Debug.Log("Performing sprint");
            sprintTimer = 0;

            // bool facingRight = rigidbody.linearVelocityX > 0;
            // int sprintDirection = facingRight ? 1 : -1;

            rigidbody.AddForce(transform.forward * sprintStrength); //, ForceMode2D.Impulse);
        }
    }

    IEnumerator FreezeMovement(float seconds)
    {
        movementFrozen = true;
        while(true){
            yield return new WaitForSeconds(seconds);

            movementFrozen = false;
        }
    }

    void PerformWallJump(int direction)
    {
        Debug.Log("WALLJUMP!");

        Vector2 force = new Vector2(wallJumpForce.x * direction, wallJumpForce.y);

        rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    bool IsGrounded()
    {
        Vector3 raycastOrigin = transform.position + (Vector3) groundedRaycastOffset;

        // Debug Grounded Logic
        // Debug.DrawRay(raycastOrigin, Vector2.down, Color.red);
        // Debug.DrawRay(raycastOrigin + Vector3.right * groundedGraceDistance, Vector2.down, Color.red);
        // Debug.DrawRay(raycastOrigin + Vector3.left * groundedGraceDistance, Vector2.down, Color.red);


        return GroundedRaycastHit(raycastOrigin) ||
            GroundedRaycastHit(raycastOrigin + Vector3.right * groundedGraceDistance) || 
            GroundedRaycastHit(raycastOrigin + Vector3.left * groundedGraceDistance);
    }

    void SwapPhysicsLayer()
    {
        if(gameObject.layer == 6)
        {
            gameObject.layer = 7;
        }
        else
        {
            gameObject.layer = 6;
        }
    }

    bool GroundedRaycastHit(Vector3 origin)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector3.down, groundedRaycastLength, 1 << gameObject.layer);

        return hit.collider != null;
    }
}
