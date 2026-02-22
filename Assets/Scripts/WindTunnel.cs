using System;
using UnityEngine;

public class WindTunnel : MonoBehaviour
{

    [Header("Wind Tunnel Settings")]
    public float windStrength;

    // Private Variables
    new Rigidbody2D rigidbody;
    PlayerController player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = PlayerController.instance;
        rigidbody = player.GetComponent<Rigidbody2D>();
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {

        if(!(other.tag.Equals("Player"))){
            return;
        }
        
        player.CancelJump();
        player.resetWallJump();
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(!(other.tag.Equals("Player"))){
            return;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(!(other.tag.Equals("Player"))){
            return;
        }

        //Apply wind force
        Vector2 windDirection = Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad) * Vector2.right + Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad) * Vector2.up;
        Vector2 windForce = windDirection * windStrength;
        rigidbody.AddForce(windForce, ForceMode2D.Force);

        //player.getFriction();

    }
}