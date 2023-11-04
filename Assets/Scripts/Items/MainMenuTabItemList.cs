using Scripts.Items;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MainMenuTabItemList : ItemList
{
    private Action<MainMenuTabData> _onClick;

    public static async Task<MainMenuTabItemList> CreateAsync(List<MainMenuTabData> tabDatas, Transform parent, Action<MainMenuTabData> onClick = null)
    {
        var itemList = new MainMenuTabItemList();
        await itemList.SetItems(tabDatas, parent, onClick);
        return itemList;
    }

    private async Task SetItems(List<MainMenuTabData> tabDatas, Transform parent, Action<MainMenuTabData> onClick = null)
    {
        Items = new List<Item>();
        foreach (var data in tabDatas)
        {
            var newItem = await MainMenuTabItem.CreateAsync(data, parent, OnItemSelected);
            Items.Add(newItem);
        }
        _onClick = onClick;
        OnItemSelected(Items[2] as MainMenuTabItem);
    }

    private void OnItemSelected(MainMenuTabItem tabItem)
    {
        SelecedItem = tabItem;
        _onClick?.Invoke(tabItem.Data as MainMenuTabData);
    }

    protected override Item SelecedItem
    {
        get => base.SelecedItem;
        set
        {
            MainMenuTabItem tabItem;
            if (SelecedItem != null)
            {
                tabItem = (MainMenuTabItem)SelecedItem;
                tabItem.IsSelected = false;
            }
            base.SelecedItem = value;
            value.IsSelected = true;
        }
    }
}