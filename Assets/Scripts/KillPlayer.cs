using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    public Vector2 playerRespawnPoint;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COLLISION!! YAY!!!!!!");
        if(collision.gameObject.name == "Player")
        {
            collision.transform.position = playerRespawnPoint;
        }

    }
}
