using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Scripts.Items
{
    public class ItemList : IDisposable
    {
        public List<Item> Items;
        private Item _selectedItem;
        protected virtual Item SelecedItem
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