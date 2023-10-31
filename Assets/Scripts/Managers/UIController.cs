using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    private void Start()
    {
        EnterView<GameView>();
    }

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
            if(menu.gameObject.name == menuName)
                return menu.gameObject;
        return null;
    }

    public void EnterView<T>() where T : View
    {
        string typeName = typeof(T).Name;
        foreach (var item in Menus)
        {
            if (typeName == item.GetViewName())
            {
                item.gameObject.SetActive(true);
                item.EnterView();
            }
            else
            {
                if (item.gameObject.activeInHierarchy)
                    item.ExitView();
                item.gameObject.SetActive(false);
            }
        }
    }
}
