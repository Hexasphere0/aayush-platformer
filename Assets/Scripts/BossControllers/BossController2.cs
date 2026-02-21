using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BossController2 : MonoBehaviour
{
    public int maxHp;
    int hp;

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

    [Header("Misc")]
    public Transform canvas;

    public GameObject levelTileParent;
    List<GameObject> levelTiles;

    Image image;

    public static BossController2 instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple BossController2s!");
        }
    }

    void Start()
    {
        image = GetComponent<Image>();
        levelTiles = new List<GameObject>();

        hp = maxHp;

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

            // Layer Change Attack
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

    public void TakeDamage()
    {
        hp--;
        image.fillAmount = (float) hp / maxHp;
    }
}
