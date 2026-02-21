using UnityEngine;

public class Projectile : MonoBehaviour
{
    string targetTag;
    float lifetime;

    float time = 0;

    public void Initialize(string targetTag, float lifetime)
    {
        this.targetTag = targetTag;
        this.lifetime = lifetime;
    }

    public void Update()
    {
        time += Time.deltaTime;

        if(time >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TRIGGERENTER!");
        if(collision.tag == targetTag)
        {
            Debug.Log("RESPAWN");
            // TODO (this only works against player rn)
            collision.GetComponent<PlayerController>().KillPlayer();
        }
    }

}
