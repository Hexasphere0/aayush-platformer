using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            PlayerController player = PlayerController.instance;

            collision.transform.position = player.playerRespawnPoint;

            if(player.gameObject.layer != player.respawnLayer)
            {
                player.LayerChange(new UnityEngine.InputSystem.InputAction.CallbackContext());
            }
        }

    }
}
