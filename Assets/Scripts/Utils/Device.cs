using Scripts.Managers;
using UnityEngine;

public static class Device 
{
    public static int Width => (int)UIController.instance.PopupCanvas.GetComponent<RectTransform>().GetWidth();
    public static int Height => (int)UIController.instance.PopupCanvas.GetComponent<RectTransform>().GetHeight();
    public static bool IsAndroid => Application.platform == RuntimePlatform.Android;
    public static bool IsIOS => Application.platform == RuntimePlatform.IPhonePlayer;
    public static bool IsMobile => IsIOS || IsAndroid;
    public static bool IsMacEditor => Application.platform == RuntimePlatform.OSXEditor;
    public static bool IsWindowsEditor => Application.platform == RuntimePlatform.WindowsEditor;
    public static bool IsEditor => IsMacEditor || IsWindowsEditor;
    public static bool IsMac => Application.platform == RuntimePlatform.OSXPlayer;
    public static bool IsWindows => Application.platform == RuntimePlatform.WindowsPlayer;

}
