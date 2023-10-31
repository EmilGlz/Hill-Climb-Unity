using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CarItem : Item
{
    public CarItem(CarData data, Transform parent) : base(data, parent)
    {
    }

    protected override void Load()
    {
        base.Load();
        if (Data == null)
            return;
        var data = Data as CarData;
        if (data == null) 
            return;
        var icon = Utils.FindGameObject("Icon", Instance).GetComponent<Image>();
        icon.sprite = data.icon;
        var title = Utils.FindGameObject("Title", Instance).GetComponent<TMP_Text>();
        title.text = data.itemName;
    }
}
