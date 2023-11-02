using System;
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
        public MainMenuTabItem(MainMenuTabData data, Transform parent, Action<MainMenuTabItem> onSelect) : base(data, parent)
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

        private void UpdateColor()
        {
            if(Instance == null) 
                return;
            Instance.GetComponent<Image>().color = ColorUtils.ParseHexColor(IsSelected ? SelectedItemColor : UnselectedItemColor);
        }

        protected override void Load()
        {
            base.Load();
            if (Data == null)
                return;
            var data = Data as MainMenuTabData;
            if (data == null)
                return;
            Instance.name = "TabItem " + data.viewName;
            Instance.GetComponentInChildren<TMP_Text>().text = data.title;
            Instance.GetComponent<Image>().color = data.color;
            var rect = Instance.GetComponent<RectTransform>();
            rect.sizeDelta = _itemSize;
            Instance.GetComponent<Button>().onClick.RemoveAllListeners();
            Instance.GetComponent<Button>().onClick.AddListener(OnClick);
        }

        protected override void OnClick()
        {
            base.OnClick();
            _onSelect.Invoke(this);
        }
    }
}