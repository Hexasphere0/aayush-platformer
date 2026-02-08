using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    GameObject camera;

    [SerializeField] float parallaxEffect;

    float xPosition;
    float length;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.gameObject;

        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMoved = camera.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = camera.transform.position.x * parallaxEffect;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if(distanceMoved > xPosition + length){
            xPosition = xPosition + length;
        }

        if(distanceMoved < xPosition - length){
            xPosition = xPosition - length;
        }
    }
}
