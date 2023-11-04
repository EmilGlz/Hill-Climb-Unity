using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Items
{
    public class MainMenuTabItem : Item
    {
        private Vector2 _itemSize = new Vector2(130, 50);
        protected override string PrefabName => "Prefabs/Buttons/TabButton";
        private readonly Action<MainMenuTabItem> _onSelect;

        public static async Task<MainMenuTabItem> CreateAsync(MainMenuTabData data, Transform parent, Action<MainMenuTabItem> onSelect)
        {
            var item = new MainMenuTabItem(data, parent, onSelect);
            await item.Load();
            return item;
        }

        private MainMenuTabItem(MainMenuTabData data, Transform parent, Action<MainMenuTabItem> onSelect) : base(data, parent)
        {
            _onSelect = onSelect;
        }

        public override bool IsSelected
        {
            get => base.IsSelected;
            set
            {
                base.IsSelected = value;
                UpdateColor();
                UpdatePosition();
            }
        }

        private string SelectedItemColor
        {
            get
            {
                if (!(Data is MainMenuTabData data))
                    return null;
                return data.viewName == "GameView" ? "#4EA023" : "#1481A6";
            }
        }

        private string UnselectedItemColor
        {
            get
            {
                if (!(Data is MainMenuTabData data))
                    return null;
                return data.viewName == "GameView" ? "#4EA023" : "#4B4E4F";
            }
        }

        private void UpdatePosition()
        {
            if (Instance == null)
                return;
            var selectedOffset = 5;
            var selectedButton = ButtonObject.GetComponent<RectTransform>();
            if (IsSelected)
            {
                selectedButton.SetTop(-selectedOffset);
                selectedButton.SetBottom(selectedOffset);
            }
            else
            {
                selectedButton.SetTop(0);
                selectedButton.SetBottom(0);
            }

        }

        private void UpdateColor()
        {
            if(Instance == null) 
                return;
            ButtonObject.GetComponent<Image>().color = ColorUtils.ParseHexColor(IsSelected ? SelectedItemColor : UnselectedItemColor);
        }

        private GameObject ButtonObject => Utils.FindGameObject("Button", Instance);

        protected override async Task Load()
        {
            await base.Load();
            if (Data == null)
                return;
            var data = Data as MainMenuTabData;
            if (data == null)
                return;
            Instance.name = "TabItem " + data.viewName;
            Instance.GetComponentInChildren<TMP_Text>().text = data.title;
            Rect.sizeDelta = _itemSize;
            var buttonImage = ButtonObject.GetComponent<Image>();
            buttonImage.color = data.color;
            var button = buttonImage.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }

        protected override void OnClick()
        {
            base.OnClick();
            _onSelect.Invoke(this);
        }
    }
}