using System.Collections.Generic;
using UnityEngine;

public class CarItemList: ItemList
{
    public CarItemList(List<CarData> carDatas, Transform parent) : base(parent)
    {
        Items = new List<Item>();
        foreach (var carData in carDatas)
            Items.Add(new CarItem(carData, parent));
    }
}
