    using UnityEngine;

public class HeadBumpJumpCancel : MonoBehaviour
{
    void OEnter2D(Collider2D collision)
    {
        PlayerJump.instance.cancelJump();
    }
}
