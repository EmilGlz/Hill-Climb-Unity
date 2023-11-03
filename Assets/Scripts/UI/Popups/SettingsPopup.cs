using Scripts.Managers;
using System;
namespace Scripts.UI.Popups
{
    public class SettingsPopup : Popup
    {
        protected override string PrefabName => "Prefabs/Popups/PausePopup";

        public SettingsPopup(Action onClose = null, Action onPopupShowed = null, bool addBackground = true, bool closeOnBackground = true) 
            : base(onClose, onPopupShowed, addBackground, closeOnBackground)
        {
        }

        public static void Create(Action onClose = null, Action onPopupShowed = null, bool addBackground = true, bool closeOnBackground = true)
        {
            if (PopupsManager.IsShown<SettingsPopup>())
                return;
            var popup = new SettingsPopup(onClose, onPopupShowed, addBackground, closeOnBackground);
            popup.Show();
        }
    }
}