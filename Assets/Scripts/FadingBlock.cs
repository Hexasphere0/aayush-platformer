using UnityEngine;

public class FadingBlock : MonoBehaviour
{
    public float timeToFade;

    float startTime;

    Collider2D blockCollider;
    SpriteRenderer spriteRenderer;
    Color color;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blockCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;

        startTime = Time.time;

        PlayerController.OnPlayerDeath += Reset;
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = 1 - ((Time.time - startTime) / timeToFade);

        if(alpha <= 0)
        {
            blockCollider.enabled = false;
        }
        else{
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }

    public void Reset()
    {
        blockCollider.enabled = true;
        startTime = Time.time;
    }
}
