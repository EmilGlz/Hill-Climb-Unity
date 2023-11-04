using Scripts.UI;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace Scripts.Items
{
    public class StagesList : ItemList
    {
        public static async Task<StagesList> CreateAsync(List<StageData> stageDatas, Transform parent, ScrollScaleController scrollView)
        {
            var itemList = new StagesList();
            await itemList.SetItems(stageDatas, parent, scrollView);
            return itemList;
        }
        private async Task SetItems(List<StageData> stageDatas, Transform parent, ScrollScaleController scrollView)
        {
            Items = new List<Item>();
            foreach (var stageData in stageDatas)
            {
                var newItem = await StageItem.CreateAsync(stageData, parent, scrollView);
                Items.Add(newItem);

            }
        }
    }
}