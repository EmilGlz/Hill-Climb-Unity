using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New User Data", menuName = "Item/Create New User Data")]
public class UserData : ScriptableObject
{
    public List<CarData> ownedCars = new List<CarData>();
    public List<StageData> stages = new List<StageData>();
    public int budget;
    public CarData currentSelectedCar;
    public StageData currentSelectedStage;
}