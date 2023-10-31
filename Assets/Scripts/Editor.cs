using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

internal class Editor 
{
    [MenuItem("Tests/Make Fuel Infinite")]
    public static void MakeFuelInfinite()
    {
        Settings.InfiniteFuelOn = true;
    }
}
