using UnityEngine;

public class ToggleGameTimer : MonoBehaviour
{
    public void ToggleGameTimerVisibility()
    {
        GameTimer.instance.ToggleGameTimerVisibility();
    }
}
