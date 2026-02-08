using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadLevel(String level)
    {
        SceneManager.LoadScene(level);
    }
}
