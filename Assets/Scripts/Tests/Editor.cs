using Scripts.Managers;
using UnityEditor;

internal class Editor 
{
#if UNITY_EDITOR
    [MenuItem("Hill Climb/Make Fuel Infinite")]
    public static void MakeFuelInfinite()
    {
        Settings.InfiniteFuelOn = true;
    }

    [MenuItem("Editor Buttons/Delete User From Local Storage")]
    private static void DeleteUserDatasFromLocal()
    {
        Settings.RemoveUserData();
    }
#endif
}
