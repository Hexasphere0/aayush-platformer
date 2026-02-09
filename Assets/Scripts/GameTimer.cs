using UnityEngine;
using TMPro;
using System.Threading;

public class GameTimer : MonoBehaviour
{
    TMP_Text text;

    float time;
    bool timerRunning = true;

    public static GameTimer instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple GameTimers!");
        }
    }

    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    static float RoundFloat(float value, int decimalPlaces)
    {
        float multiplier = Mathf.Pow(10f, decimalPlaces);
        return Mathf.Round(value * multiplier) / multiplier;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void Restart()
    {
        time = 0;
    }

    void Update()
    {
        if(timerRunning){        
            time += Time.deltaTime;
            text.text = RoundFloat(time, 2) + "s";
        }
    }
}
