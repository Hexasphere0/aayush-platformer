using UnityEngine;

public class HeadBumpJumpCancel : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerJump.instance.CancelJump();
    }
}
