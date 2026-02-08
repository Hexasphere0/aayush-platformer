using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempPlayerController : MonoBehaviour
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
    public float jumpGravityScale;
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
    }

    void FixedUpdate()
    {
        // Movement
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        if(!movementFrozen){
            Vector2 moveVector = new Vector2(moveValue.x, 0);

            rigidbody.AddForce(moveVector, ForceMode2D.Force);
        }
    }
}
