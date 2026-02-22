using System.Collections;
using UnityEngine;

public class CrumblingBlock : MonoBehaviour
{
    public float timeBeforeCrumble;
    public float regenerationTime = -1;

    Collider2D blockCollider;
    SpriteRenderer spriteRenderer;
    Color originalColor;

    void Start()
    {
        blockCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            StartCoroutine(DelayedCrumble());
        }
    }

    IEnumerator DelayedCrumble()
    {
        yield return new WaitForSeconds(timeBeforeCrumble);

        blockCollider.enabled = false;

        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        if(regenerationTime > 0){
            StartCoroutine(Regenerate());
        }
    }

    IEnumerator Regenerate()
    {
        yield return new WaitForSeconds(regenerationTime);

        blockCollider.enabled = true;

        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
    }
}
