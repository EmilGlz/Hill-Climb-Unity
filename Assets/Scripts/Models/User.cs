using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New User Data", menuName = "UserData/Create New User Data")]
public class UserData : ScriptableObject
{
    public List<CarData> ownedCars = new List<CarData>();
    public int budget;
    public CarData currentSelectedCar;
}