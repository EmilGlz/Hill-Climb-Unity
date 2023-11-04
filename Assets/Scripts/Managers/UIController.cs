using Scripts.UI;
using Scripts.UI.Popups;
using Scripts.Views;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        public void OpenGameOverMenu(string title)
        {
            EnterView<GameOverView>();
            var menu = GetMenuByName("GameOverCanvas");
            var titleText = Utils.FindGameObject("TitleText", menu).GetComponent<TMP_Text>();
            titleText.text = title;
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
                    LoadingPopup.Show();
                    await Task.Delay((int)(LoadingPopup.AnimTime * 1000));
                    await item.EnterView();
                    LoadingPopup.CloseAnim();
                }
                else
                    item.ExitView();
            }
        }
    }
}