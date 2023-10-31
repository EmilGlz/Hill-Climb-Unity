using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName ="Item/Create New Item")]
public class CarData : ScriptableObject
{
    public string id;
    public string itemName;
    public Sprite icon;
}
