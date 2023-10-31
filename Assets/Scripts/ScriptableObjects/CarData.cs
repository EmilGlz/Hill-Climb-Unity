using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName ="Item/Create New Item")]
[System.Serializable]
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
