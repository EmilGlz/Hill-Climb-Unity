using Scripts.UI;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace Scripts.Items
{
    public class CarItemList : ItemList
    {
        public static async Task<CarItemList> CreateAsync(List<CarData> carDatas, Transform parent, ScrollScaleController scrollView)
        {
            var itemList = new CarItemList();
            await itemList.SetItems(carDatas, parent, scrollView);
            return itemList;
        }

        private async Task SetItems(List<CarData> carDatas, Transform parent, ScrollScaleController scrollView)
        {
            Items = new List<Item>();
            foreach (var carData in carDatas)
            {
                var newItem = await CarItem.CreateAsync(carData, parent, scrollView);
                Items.Add(newItem);
            }
        }
    }
}