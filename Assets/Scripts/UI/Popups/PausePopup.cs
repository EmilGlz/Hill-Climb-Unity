using Scripts.Managers;
using Scripts.Views;
using System;
using UnityEngine.UI;

namespace Scripts.UI.Popups
{
    public class PausePopup : Popup
    {
        private const string prefabPath = "Prefabs/Popups/PausePopup";
        public PausePopup(Action onClose = null, Action onPopupShowed = null, bool addBackground = true, bool closeOnBackground = true)
            : base(prefabPath, onClose, onPopupShowed, addBackground, closeOnBackground)
        {
        }

        public static void Create(Action onClose = null, Action onPopupShowed = null, bool addBackground = true, bool closeOnBackground = true)
        {
            if (PopupsManager.IsShown<PausePopup>())
                return;
            var popup = new PausePopup(onClose, onPopupShowed, addBackground, closeOnBackground);
            popup.Show();
        }
        protected override void Show()
        {
            if (ItemTemplate == null)
                return;
            BackButton = Utils.FindGameObject("CloseButton", ItemTemplate).GetComponent<Button>();
            var resumeButton = Utils.FindGameObject("ResumeButton", ItemTemplate).GetComponent<Button>();
            resumeButton.onClick.AddListener(Dispose);
            var restartButton = Utils.FindGameObject("RestartButton", ItemTemplate).GetComponent<Button>();
            restartButton.onClick.AddListener(RestartClicked);
            var exitButton = Utils.FindGameObject("ExitButton", ItemTemplate).GetComponent<Button>();
            exitButton.onClick.AddListener(ExitClicked);
            base.Show();
        }
        private void RestartClicked()
        {
            _onClose += UIController.instance.RestartGame;
            Dispose();
        }
        private void ExitClicked()
        {
            UIController.instance.EnterView<MainMenuView>();
            Dispose();
        }
    }
}