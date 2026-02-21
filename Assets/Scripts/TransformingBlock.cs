using System;
using UnityEngine;

public class TransformingBlock : MonoBehaviour
{
    public GameObject innerBlock;

    public bool isTransforming;
    float transformingStartTime;
    public float transformingTime;

    public void StartTransforming()
    {
        transformingStartTime = Time.time;
        isTransforming = true;
    }

    void Update()
    {
        if (isTransforming)
        {
            if(Time.time - transformingStartTime >= transformingTime)
            {
                GetComponent<LayerChanger>().ChangeLayerNonWhite();
                isTransforming = false;

                innerBlock.GetComponent<LayerChanger>().ChangeLayerNonWhite();
                innerBlock.transform.localScale = new Vector3(0, 0, 0);
            }
            else{
                float transformingScale = (Time.time - transformingStartTime) / transformingTime;
                innerBlock.transform.localScale = new Vector3(transformingScale, transformingScale, transformingScale);
            }
        }
    }
}
