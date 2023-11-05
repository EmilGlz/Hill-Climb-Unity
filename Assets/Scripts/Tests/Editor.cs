using UnityEditor;

internal class Editor 
{
#if UNITY_EDITOR
    [MenuItem("Hill Climb/Make Fuel Infinite")]
    public static void MakeFuelInfinite()
    {
        Settings.InfiniteFuelOn = true;
    }

    [MenuItem("Hill Climb/Make Fuel Finite")]
    public static void MakeFuelFinite()
    {
        Settings.InfiniteFuelOn = false;
    }

    [MenuItem("Hill Climb/Make Head Immortal")]
    public static void MakeHeadImmortal()
    {
        Settings.HeadImmortal = true;
    }

    [MenuItem("Hill Climb/Make Head Non Immortal")]
    public static void MakeHeadNonImmortal()
    {
        Settings.HeadImmortal = false;
    }

#endif
}
