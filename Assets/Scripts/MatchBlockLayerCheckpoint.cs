using System.Linq.Expressions;
using UnityEngine;

public class MatchBlockLayerCheckpoint : Checkpoint
{
    public GameObject block;
    PlayerController playerController;

    void Start()
    {
        playerController = PlayerController.instance;
    }

    public void FixedUpdate()
    {
        playerController.respawnLayer = block.layer;
    }
}
