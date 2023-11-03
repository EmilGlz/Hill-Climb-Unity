using UnityEngine;
[CreateAssetMenu(fileName = "New Stage Data", menuName = "Item/Create New Stage Data")]
public class StageData : ScriptableObject
{
    public string id;
    public string itemName;
    public Sprite icon;
    public bool isOpened;
    public int price;
    public string description;
    public string prefabPath;

    public int currentRecord;
}
