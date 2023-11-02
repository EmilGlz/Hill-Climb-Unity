using Scripts.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTabItemList : ItemList
{
    private readonly Action<MainMenuTabData> _onClick;
    public MainMenuTabItemList(List<MainMenuTabData> tabDatas, Transform parent, Action<MainMenuTabData> onClick = null)
    {
        Items = new List<Item>();
        foreach (var data in tabDatas)
            Items.Add(new MainMenuTabItem(data, parent, OnItemSelected));
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