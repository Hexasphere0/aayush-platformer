using UnityEngine;

public class RestartButton : MonoBehaviour
{
    GameTimer gameTimer;
    PlayerController playerController;

    void Start()
    {
        gameTimer = GameTimer.instance;
        playerController = PlayerController.instance;
    }

    public void Restart()
    {
        gameTimer.Restart();
        playerController.Respawn();
    }
}
