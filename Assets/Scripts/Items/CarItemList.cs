using System.Collections.Generic;
using UnityEngine;
namespace Scripts.Items
{
    public class CarItemList : ItemList
    {
        public CarItemList(List<CarData> carDatas, Transform parent) 
        {
            Items = new List<Item>();
            foreach (var carData in carDatas)
                Items.Add(new CarItem(carData, parent));
        }
    }
}