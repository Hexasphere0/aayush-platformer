using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            collision.transform.position = PlayerController.instance.playerRespawnPoint;

            if(PlayerController.instance.gameObject.layer == 7)
            {
                PlayerController.instance.LayerChange(new UnityEngine.InputSystem.InputAction.CallbackContext());
            }
        }

    }
}
