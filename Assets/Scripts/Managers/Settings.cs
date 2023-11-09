using Scripts.Managers;
using System;
using UnityEngine;

public static class Settings
{
    public static Action OnPurchase;
    public static Action OnFuelCollected;
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
    public static bool HeadImmortal
    {
        get
        {
            var res = PlayerPrefs.GetInt("HeadImmortal");
            return res == 1;
        }
        set
        {
            var res = value ? 1 : 0;
            PlayerPrefs.SetInt("HeadImmortal", res);
        }
    }
    public static bool ShowPedalsInEditor
    {
        get
        {
            var res = PlayerPrefs.GetInt("ShowPedalsInEditor");
            return res == 1;
        }
        set
        {
            var res = value ? 1 : 0;
            PlayerPrefs.SetInt("ShowPedalsInEditor", res);
        }
    }
    private static bool _showPedals;
    public static bool ShowPedals
    {
        get => _showPedals;
        set => _showPedals = value;
    }

    private static UserData _user;
    public static UserData User
    {
        get => _user;
        set => _user = value;
    }

    public static UserData ResettedUser()
    {
        return new UserData()
        {
            ownedCars = ItemController.instance.userData.ownedCars
        };
    }
}
