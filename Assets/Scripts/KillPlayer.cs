using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    public Vector2 playerRespawnPoint;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            collision.transform.position = playerRespawnPoint;

            if(PlayerController.instance.gameObject.layer == 7)
            {
                PlayerController.instance.LayerChange(new UnityEngine.InputSystem.InputAction.CallbackContext());
            }
        }
    }
}
