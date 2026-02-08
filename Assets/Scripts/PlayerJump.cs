using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpStrength;
    public float jumpHangVelocityTreshold;

    [Header("Wall Jump")]
    public Vector2 wallJumpForce;
    public float wallJumpRaycastLength;
    public float wallJumpMovementFreezeTime;
    public Vector2 wallJumpRaycastOffset;

    // private variables
    new Rigidbody2D rigidbody;
    PlayerController player;

    InputAction jumpAction;

    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        player = GetComponent<PlayerController>();

        jumpAction = InputSystem.actions.FindAction("Jump");
        jumpAction.performed += PerformJump;
    }

    void Update()
    {
        if (jumpAction.WasReleasedThisFrame())
        {
            player.SetJumpCut(true);
        }
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
        if (player.IsGrounded())
        {
            Debug.Log("JUMPING");
            Vector2 jumpVector = Vector2.up * jumpStrength;
            player.FreezeMovement(wallJumpMovementFreezeTime);

            rigidbody.AddForce(jumpVector); //, ForceMode2D.Impulse);
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
        return jumpAction.IsPressed() && Mathf.Abs(rigidbody.linearVelocityY) <= jumpHangVelocityTreshold;
    }
}
