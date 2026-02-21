using UnityEngine;

public class ToggleSpeedrunMode : MonoBehaviour
{
    public void Toggle()
    {
        PlayerController.instance.ToggleSpeedrunMode();
    }
}
