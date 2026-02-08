using UnityEngine;

public class HeadBumpJumpCancel : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HEADBUMP");
        PlayerJump.instance.cancelJump();
    }
}
