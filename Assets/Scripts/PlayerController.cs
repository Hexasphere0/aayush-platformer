using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed;
    public float maxWalkingSpeed;

    [Header("Gravity Settings")]
    public Vector2 gravity;

    [Header("Grounded Settings")]
    public float groundedRaycastLength;
    public Vector2 groundedRaycastOffset;

    [Header("Color Settings")]
    public Color redColor;
    public Color blueColor;

    [Header("Clamp Settings")]
    public float maxFallSpeed;
    
    [Header("Friction Settings")]
    public float frictionAcceleration;

    [Header("Depricated")]
    public float groundedGraceDistance;

    // Private Variables
    private bool movementFrozen { get; set; } = false;
    private bool leftMovementFrozen { get; set; } = false;
    private bool rightMovementFrozen { get; set; } = false;
    private bool frictionFrozen { get; set; } = false;
    private bool gravityFrozen { get; set; } = false;

    new Rigidbody2D rigidbody;
    SpriteRenderer spriteRenderer;

    PlayerJump jump;

    InputAction moveAction;
    InputAction layerChangeAction;

    // Events
    public delegate void LayerChangeEvent();
    public static event LayerChangeEvent OnLayerChange;

    // Instance
    public static PlayerController instance;

    Vector2 initialRespawnPoint;
    [System.NonSerialized] public Vector2 respawnPoint;
    public int respawnLayer;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple PlayerControllers!");
        }
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        jump = GetComponent<PlayerJump>();
        
        moveAction = InputSystem.actions.FindAction("Move");

        layerChangeAction = InputSystem.actions.FindAction("LayerChange");

        layerChangeAction.performed += LayerChange;

        OnLayerChange += jump.OnLayerChange;

        respawnPoint = transform.position;
        initialRespawnPoint = transform.position;
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        // Apply Friction
        Vector2 friction = getFriction(frictionAcceleration, dt, rigidbody.linearVelocity);
        if(frictionFrozen){
            friction = Vector2.zero;
        }

        rigidbody.AddForce(friction, ForceMode2D.Impulse);

        // Movement
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        if(!movementFrozen && rigidbody.linearVelocityX * moveValue.x <= maxWalkingSpeed){
            
            if(leftMovementFrozen && moveValue.x < 0)
            {
                moveValue.x = 0;

            }
            if(rightMovementFrozen && moveValue.x > 0)
            {
                moveValue.x = 0;
            }


            Vector2 moveVector = new Vector2(moveValue.x * movementSpeed, 0);

            rigidbody.AddForce(moveVector, ForceMode2D.Force);
            rigidbody.AddForce(-friction * Mathf.Abs(moveValue.x), ForceMode2D.Impulse);
        }

        // Gravity
        if(!(gravityFrozen)){
            rigidbody.AddForce(gravity, ForceMode2D.Force);

        }
        
        // Clamp Fall Speed
        rigidbody.linearVelocityY = Mathf.Max(rigidbody.linearVelocityY, -maxFallSpeed);

        // Clamp Movement Speed
        int direction = rigidbody.linearVelocityX > 0 ? 1 : -1;
    }

    public IEnumerator FreezeMovement(float seconds)
    {
        movementFrozen = true;

        yield return new WaitForSeconds(seconds);
        movementFrozen = false;
    }

    public IEnumerator FreezeLeftMovement(float seconds)
    {
        leftMovementFrozen = true;

        yield return new WaitForSeconds(seconds);
        leftMovementFrozen = false;

    }
    public IEnumerator FreezeRightMovement(float seconds)
    {
        
        rightMovementFrozen = true;

        yield return new WaitForSeconds(seconds);
        rightMovementFrozen = false;
    }

    public IEnumerator FreezeFriction(float seconds)
    {

        frictionFrozen = true;

        yield return new WaitForSeconds(seconds);
        frictionFrozen = false;
    }

    public IEnumerator FreezeGravity(float seconds)
    {
        gravityFrozen = true;

        yield return new WaitForSeconds(seconds);
        gravityFrozen = false;
    }

    public bool IsGrounded()
    {
        Vector3 raycastOrigin = transform.position + (Vector3) groundedRaycastOffset;

        // Debug Grounded Logic
        Debug.DrawRay(raycastOrigin, Vector2.down, Color.red);
        Debug.DrawRay(raycastOrigin + Vector3.right * groundedGraceDistance, Vector2.down, Color.red);
        Debug.DrawRay(raycastOrigin + Vector3.left * groundedGraceDistance, Vector2.down, Color.red);

        return GroundedRaycastHit(raycastOrigin) ||
            GroundedRaycastHit(raycastOrigin + Vector3.right * groundedGraceDistance) || 
            GroundedRaycastHit(raycastOrigin + Vector3.left * groundedGraceDistance);
    }

    public void LayerChange(InputAction.CallbackContext context)
    {   
        if(gameObject.layer == 6)
        {
            spriteRenderer.color = blueColor;
            gameObject.layer = 7;
        }
        else
        {
            spriteRenderer.color = redColor;
            gameObject.layer = 6;
        }

        OnLayerChange();
    }

    Vector2 getFriction(float frictionAcceleration, float dt, Vector2 currentVelocity)
    {
        if(frictionAcceleration * dt > Mathf.Abs(currentVelocity.x))
        {
            return new Vector2(-currentVelocity.x, 0);
        }
        else
        {
            return new Vector2(-frictionAcceleration * dt * Mathf.Sign(currentVelocity.x), 0);

        }
    }

    bool GroundedRaycastHit(Vector3 origin)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector3.down, groundedRaycastLength, GetInteractableLayers());

        return hit.collider != null;
    }

    public int GetInteractableLayers()
    {
        string currentLayer = LayerMask.LayerToName(gameObject.layer);
        string[] layers = new string[] {currentLayer, "Black"};

        return LayerMask.GetMask(layers);
    }

    public void Respawn()
    {
        transform.position = respawnPoint;

        if(gameObject.layer != respawnLayer)
        {
            LayerChange(new UnityEngine.InputSystem.InputAction.CallbackContext());
        }
    }

    // Respawn from the start of the level
    public void HardRespawn()
    {
        transform.position = initialRespawnPoint;

        if(gameObject.layer != respawnLayer)
        {
            LayerChange(new UnityEngine.InputSystem.InputAction.CallbackContext());
        }
    }

    public void CancelJump()
    {
        jump.CancelJump();
    }

    public void resetWallJump()
    {
        jump.resetWallJump();
    }
}
