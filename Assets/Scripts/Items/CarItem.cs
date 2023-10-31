using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Scripts.Items
{
    public class CarItem : Item
    {
        protected override string PrefabName => "Prefabs/Inventory/car-item";
        public CarItem(CarData data, Transform parent) : base(data, parent)
        {
        }

        protected override void Load()
        {
            base.Load();
            if (Data == null)
                return;
            var data = Data as CarData;
            if (data == null)
                return;
            var icon = Utils.FindGameObject("Icon", Instance).GetComponent<Image>();
            icon.sprite = data.icon;
            var title = Utils.FindGameObject("Title", Instance).GetComponent<TMP_Text>();
            title.text = data.itemName;
            Instance.GetComponent<Button>().onClick.RemoveAllListeners();
            Instance.GetComponent<Button>().onClick.AddListener(OnClick);
        }

        protected override void OnClick()
        {
            base.OnClick();
            if (Data == null) return;
            if (Data is not CarData carData)
                return;
            if(carData.level == 0)
                Buy(carData);
            else
                Equip(carData);
        }

        private void Buy(CarData carData)
        {
            UnlockCarPopup.Create(carData);
        }

        private void Equip(CarData carData)
        {

        }
    }
}