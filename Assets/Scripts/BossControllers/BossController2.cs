using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BossController2 : MonoBehaviour
{
    public float timeBetweenTicks;
    float timeSinceTick = 0f;
    int ticks = 0;

    [Header("Color Change Attack Settings")]
    public float ticksBetweenColorChange;
    public int colorChangeCount;
    
    public GameObject levelTileParent;
    List<GameObject> levelTiles;

    void Start()
    {
        levelTiles = new List<GameObject>();

        foreach (Transform childTransform in levelTileParent.transform)
        {
            GameObject childObject = childTransform.gameObject;
            levelTiles.Add(childObject);
        }
    }
    
    void FixedUpdate()
    {
        timeSinceTick += Time.fixedDeltaTime;

        if(timeSinceTick >= timeBetweenTicks)
        {
            ticks++;
            timeSinceTick = 0;

            // Projectile Attack
            if(ticks % ticksBetweenColorChange == 0)
            {
                for(int i = 0; i < colorChangeCount; i++)
                {
                    int objectIndex = UnityEngine.Random.Range(0, levelTiles.Count);
                    Debug.Log(objectIndex);
                    levelTiles[objectIndex].GetComponent<LayerChanger>().ChangeLayerNonWhite();
                }
            }
        }
    }
}
