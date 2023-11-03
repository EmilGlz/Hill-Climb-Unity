using Scripts.Managers;
using Scripts.Views;
using TMPro;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UI;
namespace Scripts.Items
{
    public class StageItem : Item
    {
        protected override string PrefabName => "Prefabs/Inventory/stage-item";
        public StageItem(StageData data, Transform parent) : base(data, parent)
        {
        }

        protected override void Load()
        {
            base.Load();
            if (Data == null)
                return;
            var data = Data as StageData;
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
            if (Data is not StageData stageData)
                return;
            if (!stageData.isOpened)
                Buy(stageData);
            else
                Equip(stageData);
        }

        private void Buy(StageData carData)
        {
            //UnlockCarPopup.Create(carData);
        }

        private void Equip(StageData carData)
        {
            Settings.User.currentSelectedStage = carData;
            UIController.instance.EnterView<GameView>();
        }

        private void UpdateLockState(bool isLocked)
        {
            var data = Data as StageData;
            if (data == null)
                return;
            var lockIcon = Utils.FindGameObject("LockedIcon", Instance);
            var pricePanel = Utils.FindGameObject("PricePanel", Instance);
            var darkOverlay = Utils.FindGameObject("DarkOverlay", Instance);
            var recordPanel = Utils.FindGameObject("RecordPanel", Instance);
            var priceText = Utils.FindGameObject("PriceText", Instance).GetComponent<TMP_Text>();
            var recordText = Utils.FindGameObject("RecordText", Instance).GetComponent<TMP_Text>();
            lockIcon.SetActive(isLocked);
            pricePanel.SetActive(isLocked);
            darkOverlay.SetActive(isLocked);
            recordPanel.SetActive(!isLocked);
            priceText.text = data.price.ToString("#,##0");
            recordText.text = data.currentRecord.ToString("#,##0");
        }
    }
}