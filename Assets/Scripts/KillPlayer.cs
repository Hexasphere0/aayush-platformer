using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            PlayerController.instance.KillPlayer();
        }
    }
}
