using UnityEngine;
using TMPro;

public class LayerChanger : MonoBehaviour
{
    public Color redColor;
    public Color blueColor;
    public Color whiteColor;

    public void ChangeLayer()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if(gameObject.layer == 6)
        {   
            gameObject.layer = 7;
            spriteRenderer.color = blueColor; 
        }
        else if(gameObject.layer == 7)
        {
            gameObject.layer = 8;
            spriteRenderer.color = whiteColor; 
        }
        else
        {
            gameObject.layer = 6;
            spriteRenderer.color = redColor;
        }
    }
}