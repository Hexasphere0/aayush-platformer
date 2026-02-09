using UnityEngine;
using TMPro;

public class LevelBlockEditor : MonoBehaviour
{
    public Color redColor;
    public Color blueColor;

    public void ChangeLayer()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if(gameObject.layer == 6)
        {   
            gameObject.layer = 7;
            spriteRenderer.color = blueColor; 
        }
        else
        {
            gameObject.layer = 6;
            spriteRenderer.color = redColor; 
        }
    }
}