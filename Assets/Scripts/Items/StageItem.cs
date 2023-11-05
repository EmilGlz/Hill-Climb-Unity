using Scripts.Managers;
using Scripts.UI;
using Scripts.Views;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Scripts.Items
{
    public class StageItem : Item, IDarker, IScaler
    {
        private readonly ScrollScaleController _scrollView;

        protected override string PrefabName => "Prefabs/Inventory/stage-item";

        public float MinimumScale => 0.3f;

        public float MaximumScale => 1f;

        public float DarkestAlphaValue => 0.9f;

        public float LightestAlphaValue => Data is StageData stageData && !stageData.isOpened ? 0.4f : 0f;

        public static async Task<StageItem> CreateAsync(StageData data, Transform parent, ScrollScaleController scrollView)
        {
            var item = new StageItem(data, parent, scrollView);
            await item.Load();
            return item;
        }

        private StageItem(StageData data, Transform parent, ScrollScaleController scrollView) : base(data, parent)
        {
            _scrollView = scrollView;
        }

        protected override async Task Load()
        {
            await base.Load();
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

        public async void Reload()
        {
            if (Data == null)
                return;
            if (!(Data is StageData stageData))
                return;
            var stageDataOfUser = Settings.User.stages.FirstOrDefault(s => s.id == stageData.id);
            if (stageDataOfUser == null)
                return;
            Data = stageDataOfUser;
            await Load();
        }

        protected override void OnClick()
        {
            base.OnClick();
            if (Data == null) return;
            if (Data is not StageData stageData)
                return;

            if (!ScrollScaleController.InMiddle(this))
            {
                _scrollView.StartCoroutine(_scrollView.ScrollTo(this));
                return;
            }

            if (!stageData.isOpened)
                Buy(stageData);
            else
                Equip(stageData);
        }

        private void Buy(StageData stageData)
        {
            UnlockPopup.Create(stageData, Reload);
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

        public void UpdateOverlay(float alphaValue)
        {
            var overlay = Utils.FindGameObject("DarkOverlay", Instance);
            overlay.SetActive(true);
            alphaValue = Mathf.Clamp(alphaValue, LightestAlphaValue, DarkestAlphaValue);
            overlay.GetComponent<Image>().color = new Color(0, 0, 0, alphaValue);
        }

        public void UpdateScale(float scaleValue)
        {
            scaleValue = Mathf.Clamp(scaleValue, MinimumScale, MaximumScale);
            Instance.transform.localScale = Vector3.one * scaleValue;
        }
    }
}