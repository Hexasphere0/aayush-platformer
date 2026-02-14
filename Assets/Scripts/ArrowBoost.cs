using System.Collections;
using UnityEngine;

public class ArrowBoost : MonoBehaviour
{
    [Header("Boost Velocity")]
    public float boostVelocity;
    public float boostDuration;

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

        Vector2 parallelComponent = Vector2.Dot(boostDirection, rigidbody.linearVelocity) * boostDirection;
        rigidbody.AddForce(-(rigidbody.linearVelocity - parallelComponent), ForceMode2D.Impulse);

        if(Vector2.Dot(boostDirection, parallelComponent) < 0){
            rigidbody.AddForce(-parallelComponent, ForceMode2D.Impulse);
        }

        Vector2 impulse = boostDirection * boostVelocity;

        rigidbody.AddForce(impulse, ForceMode2D.Impulse);

        StartCoroutine(player.FreezeMovement(boostDuration));
        StartCoroutine(player.FreezeFriction(boostDuration));
        StartCoroutine(player.FreezeGravity(boostDuration));

        StartCoroutine(endBoost(impulse));
    }

    IEnumerator endBoost(Vector2 impulse)
    {
        yield return new WaitForSeconds(boostDuration);

        rigidbody.AddForce(new Vector2(0, -impulse.y/2f), ForceMode2D.Impulse);
    }
}
