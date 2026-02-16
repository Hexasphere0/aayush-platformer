using System.Collections;
using UnityEngine;

public class ArrowBoost : MonoBehaviour
{
    [Header("Boost Velocity")]
    public float boostVelocity;
    public float boostDuration;

    //Static variables
    static float timeTillBoostEnd = 0f;
    static Vector2 endBoostVelocity = Vector2.zero;
    static float numInstances = 0f;
    // Private Variables
    new Rigidbody2D rigidbody;
    PlayerController player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = PlayerController.instance;
        rigidbody = player.GetComponent<Rigidbody2D>();
        numInstances++;
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        player.CancelJump();
        player.resetWallJump();
        
        if(!(other.tag.Equals("Player"))){
            return;
        }

        Vector2 boostDirection = Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad) * Vector2.right + Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad) * Vector2.up;
        
        
        rigidbody.AddForce(-rigidbody.linearVelocity, ForceMode2D.Impulse);

        Vector2 impulse = boostDirection * boostVelocity;

        rigidbody.AddForce(impulse, ForceMode2D.Impulse);

        StartCoroutine(player.FreezeMovement(boostDuration));
        StartCoroutine(player.FreezeFriction(boostDuration));
        StartCoroutine(player.FreezeGravity(boostDuration));

        timeTillBoostEnd = boostDuration;
        endBoostVelocity = new Vector2(-(impulse.x * (1 - Mathf.Pow(3f/4f, boostVelocity/30))), -(impulse.y * (1 - Mathf.Pow(2f/5f, boostVelocity/30))));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeTillBoostEnd -= Time.fixedDeltaTime/numInstances;

        if(timeTillBoostEnd < 0)
        {
            timeTillBoostEnd = 0;

        }

        if(timeTillBoostEnd == 0 && endBoostVelocity != Vector2.zero)
        {
            rigidbody.AddForce(endBoostVelocity, ForceMode2D.Impulse);
            endBoostVelocity = Vector2.zero;
        }
    }
}
