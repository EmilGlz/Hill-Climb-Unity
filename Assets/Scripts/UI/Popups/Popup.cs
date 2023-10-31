using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Scripts.Managers;
namespace Scripts.UI.Popups
{
    public abstract class Popup : IDisposable
    {
        protected Action _onClose;
        private readonly Action _onPopupShowed;
        private readonly bool _addBackground;
        private readonly bool _closeOnBackground;
        private const string _backgroundImagePrefab = "Prefabs/Popups/PopupBackground";
        protected GameObject BackgroundImage;
        protected Button BackButton;
        public GameObject ItemTemplate;
        protected virtual float AnimationTime => .25f;
        protected Popup(string prefabName, Action onClose = null, Action onPopupShowed = null, bool addBackground = true, bool closeOnBackground = true)
        {
            ItemTemplate = ResourceHelper.InstantiatePrefab(prefabName, UIController.instance.PopupCanvas);
            if (ItemTemplate != null)
                PopupsManager.instance.AddPopup(this);
            _onClose = onClose;
            _onPopupShowed = onPopupShowed;
            _addBackground = addBackground;
            _closeOnBackground = closeOnBackground;
        }
        public virtual void Dispose()
        {
            CloseAnimation();
        }

        protected virtual void DestroyPopup()
        {
            PopupsManager.instance.RemovePopup(this);
            if (BackgroundImage != null)
                UnityEngine.Object.Destroy(BackgroundImage);
            if (ItemTemplate != null)
                UnityEngine.Object.Destroy(ItemTemplate);
            _onClose?.Invoke();
        }

        protected virtual void Show()
        {
            if (ItemTemplate == null)
                return;
            BackgroundImage = ResourceHelper.InstantiatePrefab(_backgroundImagePrefab, UIController.instance.PopupCanvas);
            BackgroundImage.transform.SetAsFirstSibling();
            BackgroundImage.name = GetType().Name + " Background";
            BackgroundImage.GetComponent<Image>().color = new Color(0, 0, 0, (_addBackground ? .5f : 0f));
            if (_closeOnBackground)
                BackgroundImage.GetComponent<Button>().onClick.AddListener(Dispose);
            if (BackButton != null)
                BackButton.onClick.AddListener(Dispose);
            InitAnimation();
        }

        private void InitAnimation()
        {
            if (ItemTemplate == null)
                return;
            ItemTemplate.transform.localScale = Vector3.zero;
            ItemTemplate.transform.DOScale(Vector3.one, AnimationTime).OnComplete(() =>
            {
                _onPopupShowed?.Invoke();
            });
        }

        private void CloseAnimation()
        {
            if (ItemTemplate == null)
                return;
            Time.timeScale = 1f;
            ItemTemplate.transform.localScale = Vector3.one;
            ItemTemplate.transform.DOScale(Vector3.zero, AnimationTime).OnComplete(() =>
            {
                _onClose?.Invoke();
                DestroyPopup();
            });
        }
    }
}