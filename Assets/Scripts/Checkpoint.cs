using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Color enabledColor;
    public Color disabledColor;

    public int respawnLayer;

    // win level on touch
    public bool finalCheckpoint;

    SpriteRenderer spriteRenderer;

    // You can only collect a checkpoint 1 time
    bool checkpointEnabled = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = enabledColor;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(checkpointEnabled && collision.gameObject.name == "Player")
        {
            PlayerController player = PlayerController.instance;

            player.playerRespawnPoint = transform.position;
            player.respawnLayer = respawnLayer;

            spriteRenderer.color = disabledColor;

            if (finalCheckpoint)
            {
                GameTimer.instance.StopTimer();
            }

            checkpointEnabled = false;
        }
    }
}
