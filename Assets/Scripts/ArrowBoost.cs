using UnityEngine;

public class ArrowBoost : MonoBehaviour
{
    [Header("Boost Velocity")]
    public float boostVelocity;

    // Private Variables
    new Rigidbody2D rigidbody;
    PlayerController player;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = PlayerController.instance;
        rigidbody = player.GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if(!(other.tag.Equals("Player"))){
            return;
        }

        Vector2 boostDirection = Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad) * Vector2.right + Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad) * Vector2.up;
        rigidbody.AddForce(boostDirection * boostVelocity, ForceMode2D.Impulse);
    }
}
