using Scripts.Managers;
using Scripts.UI;
using Scripts.Views;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Scripts.Items
{
    public class CarItem : Item, IDarker, IScaler
    {
        private readonly ScrollScaleController _scrollView;

        protected override string PrefabName => "Prefabs/Inventory/car-item";

        public float MinimumScale => 0.3f;

        public float MaximumScale => 1f;

        public float DarkestAlphaValue => 0.9f;

        public float LightestAlphaValue => Data is CarData carData && !carData.isOpened ? 0.4f : 0f;

        public CarItem(CarData data, Transform parent, ScrollScaleController scrollView) : base(data, parent)
        {
            _scrollView = scrollView;
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
            UpdateLockState(!data.isOpened);
            Instance.GetComponent<Button>().onClick.RemoveAllListeners();
            Instance.GetComponent<Button>().onClick.AddListener(OnClick);
        }

        protected override void OnClick()
        {
            base.OnClick();
            if (Data == null) return;
            if (Data is not CarData carData)
                return;

            if (!ScrollScaleController.InMiddle(this))
            {
                _scrollView.StartCoroutine(_scrollView.ScrollTo(this));
                return;
            }

            if (!carData.isOpened)
                Buy(carData);
            else
                Equip(carData);
        }

        private void Reload()
        {
            if (Data == null)
                return;
            if (!(Data is CarData carData))
                return;
            var carDataOfUser = Settings.User.ownedCars.FirstOrDefault(c => c.id == carData.id);
            if (carDataOfUser == null)
                return;
            Data = carDataOfUser;
            Load();
        }

        private void Buy(CarData carData)
        {
            UnlockPopup.Create(carData, Reload);
        }

        private void Equip(CarData carData)
        {
            Settings.User.currentSelectedCar = carData;
            UIController.instance.EnterView<GameView>();
        }

        private void UpdateLockState(bool isLocked)
        {
            var lockIcon = Utils.FindGameObject("LockedIcon", Instance);
            var pricePanel = Utils.FindGameObject("PricePanel", Instance);
            var darkOverlay = Utils.FindGameObject("DarkOverlay", Instance);
            var priceText = Utils.FindGameObject("PriceText", Instance).GetComponent<TMP_Text>();
            lockIcon.SetActive(isLocked);
            pricePanel.SetActive(isLocked);
            darkOverlay.SetActive(isLocked);
            priceText.text = (Data as CarData).price.ToString("#,##0");
        }

        public void UpdateOverlay(float alphaValue)
        {
            var overlay = Utils.FindGameObject("DarkOverlay", Instance);
            overlay.SetActive(true);
            alphaValue = Mathf.Clamp(alphaValue, LightestAlphaValue, DarkestAlphaValue);
            overlay.GetComponent<Image>().color = new Color(0,0,0, alphaValue);
        }

        public void UpdateScale(float scaleValue)
        {
            scaleValue = Mathf.Clamp(scaleValue, MinimumScale, MaximumScale);
            Instance.transform.localScale = Vector3.one * scaleValue;
        }
    }
}