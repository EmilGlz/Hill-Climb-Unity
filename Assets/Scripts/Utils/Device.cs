using Scripts.Managers;
using UnityEngine;

public static class Device 
{
    public static int Width => (int)UIController.instance.PopupCanvas.GetComponent<RectTransform>().GetWidth();
    public static int Height => (int)UIController.instance.PopupCanvas.GetComponent<RectTransform>().GetHeight();

}
