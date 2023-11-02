using UnityEngine;
[CreateAssetMenu(fileName = "New Car Data", menuName ="Item/Create New Car Data")]
public class CarData : ScriptableObject
{
    public string id;
    public string itemName;
    public Sprite icon;
    public int level;
    public int price;
    public string description;
    public string prefabPath;
}
