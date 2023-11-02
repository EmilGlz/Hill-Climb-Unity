using System;
using System.Collections.Generic;
namespace Scripts.Items
{
    public class ItemList : IDisposable
    {
        protected List<Item> Items;
        private Item _selectedItem;
        protected virtual Item SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; }
        }
        public virtual void Dispose()
        {
            foreach (var item in Items)
            {
                item.Dispose();
            }
        }
    }
}