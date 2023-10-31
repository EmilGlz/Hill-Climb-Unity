using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : IDisposable
{
    protected Transform Parent;
    protected List<Item> Items;

    public ItemList(Transform parent)
    {
        Parent = parent;
    }

    public virtual void Dispose()
    {
        foreach (var item in Items)
        {
            item.Dispose();
        }
    }
}
