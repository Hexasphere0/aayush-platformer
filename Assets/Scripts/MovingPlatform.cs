using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector2 leftPosition;
    public Vector2 rightPosition;

    public float timeBetweenDestinations;

    float time = 0;
    int direction = 1;
    
    void Start()
    {
        transform.position = leftPosition;
    }

    void Update()
    {
        time += Time.deltaTime * direction;
        if(time > timeBetweenDestinations || time < 0)
        {
            direction = -direction;
        }

        transform.position = Vector2.Lerp(leftPosition, rightPosition, time / timeBetweenDestinations);
    }
}
