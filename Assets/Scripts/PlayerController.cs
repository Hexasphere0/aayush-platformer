using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed;

    [Header("Dash Settings")] // Broken needs to be fixed
    public float sprintStrength;
    public float sprintCooldown;

    [Header("Gravity Settings")]
    public float hardFallGravityScale;
    public float jumpCutGravityScale;
    public float fallingGravityScale;
    public float jumpHangGravityScale;
    public float defaultGravityScale;

    [Header("Grounded Settings")]
    public float groundedRaycastLength;
    public Vector2 groundedRaycastOffset;

    [Header("Color Settings")]
    public Color redColor;
    public Color blueColor;

    [Header("Clamp Settings")]
    public float maxFallSpeed;
    public float maxMovementSpeed;

    [Header("Depricated")]
    public float groundedGraceDistance;

    // Private Variables
    private bool movementFrozen { get; set; } = false;

    new Rigidbody2D rigidbody;
    SpriteRenderer spriteRenderer;

    PlayerJump jump;

    InputAction moveAction;

    bool isJumpCut { get; set; }

    float sprintTimer = 0;
    float layerChangeCooldownTime = 0f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        jump = GetComponent<PlayerJump>();
        
        moveAction = InputSystem.actions.FindAction("Move");
        
        InputSystem.actions.FindAction("Sprint").performed += PerformSprint;

        SetGravityScale(defaultGravityScale);
    }

    void Update()
    {   
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
        else if(jump.CanJumpHang())
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

    public IEnumerator FreezeMovement(float seconds)
    {
        movementFrozen = true;
        while(true){
            yield return new WaitForSeconds(seconds);

            movementFrozen = false;
        }
    }

    public bool IsGrounded()
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

    public void SetJumpCut(bool jumpCut)
    {
        isJumpCut = jumpCut;
    }
}
