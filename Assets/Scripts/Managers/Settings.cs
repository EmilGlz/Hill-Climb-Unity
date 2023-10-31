using Scripts.Managers;
using System.IO;
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

    public static void SaveUserData()
    {
        string jsonData = JsonUtility.ToJson(User);
        File.WriteAllText(Application.persistentDataPath + "/userData.json", jsonData);
    }

    public static void LoadUserData()
    {
        string filePath = Application.persistentDataPath + "/userData.json";
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            User = JsonUtility.FromJson<UserData>(jsonData);
        }
    }

    public static void RemoveUserData()
    {
        string filePath = Application.persistentDataPath + "/userData.json";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }


}
