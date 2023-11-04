using Scripts.UI;
using System.Collections.Generic;
using UnityEngine;
namespace Scripts.Items
{
    public class StagesList : ItemList
    {
        public StagesList(List<StageData> stageDatas, Transform parent, ScrollScaleController scrollView)
        {
            Items = new List<Item>();
            foreach (var stageData in stageDatas)
                Items.Add(new StageItem(stageData, parent, scrollView));
        }
    }
}