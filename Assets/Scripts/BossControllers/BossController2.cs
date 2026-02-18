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

    [Header("Checkpoint Settings")]
    public float ticksBetweenCheckpointSpawns;
    public float checkpointSpawnCount;
    public float timeToCollectCheckpoints;
    public Vector2 checkpointSpawnOffset;
    public GameObject bossCheckpointPrefab;
    public Transform canvas;

    [Header("Misc")]
    
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
                    levelTiles[objectIndex].GetComponent<TransformingBlock>().StartTransforming();
                }
            }

            // Spawn a checkpoint for you to collect
            if(ticks % ticksBetweenCheckpointSpawns == 0)
            {
                for(int i = 0; i < checkpointSpawnCount; i++)
                {
                    int objectIndex = UnityEngine.Random.Range(0, levelTiles.Count);
                    Vector2 checkpointPosition = (Vector2) levelTiles[objectIndex].transform.position + Vector2.up * (levelTiles[objectIndex].transform.localScale / 2) + checkpointSpawnOffset;

                    GameObject checkpoint = Instantiate(bossCheckpointPrefab, checkpointPosition, Quaternion.identity, canvas);
                    checkpoint.GetComponent<BossFightCheckpoint>().Initialize(timeToCollectCheckpoints);
                }
            }
        }
    }
}
