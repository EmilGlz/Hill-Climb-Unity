using UnityEngine;

public static class Settings
{
    public static bool InfiniteFuelOn
    {
        get
        {
            var res = PlayerPrefs.GetInt("InfiniteFuel");
            return res == 1;
        }
        set
        {
            var res = value ? 1 : 0;
            PlayerPrefs.SetInt("InfiniteFuel", res);
        }
    }
}
