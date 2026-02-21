using UnityEngine;
using UnityEngine.EventSystems;

public class MenuBackground : MonoBehaviour, IPointerDownHandler
{
    public GameObject menu;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        menu.SetActive(false);
    }
}
