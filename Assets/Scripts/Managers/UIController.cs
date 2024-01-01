using Scripts.UI.Popups;
using Scripts.Views;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Scripts.Managers
{
    public class UIController : MonoBehaviour
    {
        #region Singleton
        public static UIController instance;
        private void Awake()
        {
            instance = this;
        }
        #endregion
        [SerializeField] private View[] Menus;
        [SerializeField] public Transform PopupCanvas;
        [SerializeField] public Gradient FuelGradient;

        [SerializeField] private Sprite _gasPedalPressedSprite;
        [SerializeField] private Sprite _gasPedalSprite;
        [SerializeField] private Sprite _brakePedalSprite;
        [SerializeField] private Sprite _brakePedalPressedSprite;

        public void PedalPressed(GameObject pedalImage, bool isGas)
        {
            Image img = pedalImage.GetComponent<Image>();
            if (img == null || _gasPedalPressedSprite == null || _brakePedalPressedSprite == null)
                return;
            if (isGas)
                img.sprite = _gasPedalPressedSprite;
            else
                img.sprite = _brakePedalPressedSprite;
        }

        public void PedalNotPressed(GameObject pedalImage, bool isGas)
        {
            Image img = pedalImage.GetComponent<Image>();
            if (img == null || _gasPedalPressedSprite == null || _brakePedalPressedSprite == null)
                return;
            if (isGas)
                img.sprite = _gasPedalSprite;
            else
                img.sprite = _brakePedalSprite;
        }

        public void OpenGameOverMenu(string title)
        {
            var view = GetCurrentView() as GameView;
            view.OpenGameOverMenu();
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OpenPausePopup(Action onClose = null, Action onPopupShowed = null)
        {
            PausePopup.Create(onClose, onPopupShowed);
        }

        public GameObject GetMenuByName(string menuName)
        {
            if (string.IsNullOrEmpty(menuName))
                return null;
            foreach (var menu in Menus)
                if (menu.gameObject.name == menuName)
                    return menu.gameObject;
            return null;
        }

        public async void EnterView<T>() where T : View
        {
            string typeName = typeof(T).Name;
            foreach (var item in Menus)
            {
                if (typeName == item.GetViewName())
                {
                    item.gameObject.SetActive(true);
                    await item.EnterView();
                }
                else
                    item.ExitView();
            }
        }

        public View GetCurrentView()
        {
            foreach (var item in Menus)
                if (item.gameObject.activeInHierarchy)
                    return item;
            return null;
        }

        public void UpdateBonus(int bonus)
        {
            var view = GetCurrentView();
            if (!(view is GameView gameView))
                return;
            gameView.UpdateBonus(bonus);
        }
    }
}