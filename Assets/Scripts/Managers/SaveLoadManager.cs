//using Scripts.Models;
//using System;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
//using UnityEngine;
//using static UnityEngine.UIElements.UxmlAttributeDescription;
//namespace Scripts.Managers
//{

//    public static class SaveLoadManager
//    {
//        private static string dataFilePath = Application.persistentDataPath + "/userData.dat";

//        public static void Save(User user)
//        {
//            try
//            {
//                using (FileStream file = File.Create(dataFilePath))
//                {
//                    BinaryFormatter bf = new BinaryFormatter();
//                    bf.Serialize(file, user);
//                }
//            }
//            catch (Exception e)
//            {
//                Debug.LogError("Error saving user data: " + e.Message);
//            }
//        }

//        public static User Load()
//        {
//            if (File.Exists(dataFilePath))
//            {
//                try
//                {
//                    // Load and deserialize the data
//                    using (FileStream file = File.Open(dataFilePath, FileMode.Open))
//                    {
//                        if (file.Length > 0)
//                        {
//                            BinaryFormatter bf = new BinaryFormatter();
//                            User user = (User)bf.Deserialize(file);
//                            return user;
//                        }
//                        else
//                        {
//                            Debug.LogWarning("Data file is empty.");
//                        }
//                    }
//                }
//                catch (Exception e)
//                {
//                    Debug.LogError("Error loading user data: " + e.Message);
//                }
//            }
//            else
//            {
//                Debug.LogWarning("Data file does not exist.");
//            }

//            return null;
//        }

//        public static void DeleteLocalData()
//        {
//            if (File.Exists(dataFilePath))
//            {
//                File.Delete(dataFilePath);
//            }
//        }

//        //public static void Save(User userData)
//        //{
//        //    if (userData == null)
//        //        return;
//        //    BinaryFormatter bf = new BinaryFormatter();
//        //    FileStream file = File.Create(Application.persistentDataPath + "/userData.dat");
//        //    bf.Serialize(file, userData);
//        //    file.Close();
//        //}

//        //public static User Load()
//        //{
//        //    if (File.Exists(Application.persistentDataPath + "/userData.dat"))
//        //    {
//        //            BinaryFormatter bf = new BinaryFormatter();
//        //            FileStream file = File.Open(Application.persistentDataPath + "/userData.dat", FileMode.Open);
//        //        try
//        //        {
//        //            User userData = (User)bf.Deserialize(file);
//        //            file.Close(); // Move the file stream closing here, after deserialization
//        //            return userData;
//        //        }
//        //        catch (Exception)
//        //        {
//        //            file.Close(); // Move the file stream closing here, after deserialization
//        //            return null;
//        //        }
//        //    }
//        //    return null;
//        //}

//        //public static void DeleteData()
//        //{
//        //    string dataPath = Application.persistentDataPath + "/userData.dat";

//        //    if (File.Exists(dataPath))
//        //    {
//        //        File.Delete(dataPath);
//        //        Debug.Log("UserData deleted.");
//        //    }
//        //    else
//        //    {
//        //        Debug.LogWarning("UserData file does not exist. Nothing to delete.");
//        //    }
//        //}
//    }
//}