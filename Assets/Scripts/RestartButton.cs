using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
