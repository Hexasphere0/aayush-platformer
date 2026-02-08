using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Color enabledColor;
    public Color disabledColor;

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
        if(enabled && collision.gameObject.name == "Player")
        {
            PlayerController.instance.playerRespawnPoint = transform.position;
            
            spriteRenderer.color = disabledColor;
            checkpointEnabled = false;
        }
    }
}
