using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    TMP_Text text;
    float time;

    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    public float RoundFloat(float value, int decimalPlaces)
    {
        float multiplier = Mathf.Pow(10f, decimalPlaces);
        return Mathf.Round(value * multiplier) / multiplier;
    }

    void Update()
    {
        time += Time.deltaTime;

        text.text = RoundFloat(time, 2) + "s";
    }
}
