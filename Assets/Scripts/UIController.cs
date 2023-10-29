using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIController : MonoBehaviour
{
    #region Singleton
    public static UIController instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion
    [SerializeField] private GameObject[] Menus;
    [SerializeField] public Transform PopupCanvas;
    private void Start()
    {
        OpenMenu("GameCanvas");
    }

    public void OpenGameOverMenu(string title)
    {
        OpenMenu("GameOverCanvas");
        var menu = GetMenuByName("GameOverCanvas");
        var titleText = Utils.FindGameObject("TitleText", menu).GetComponent<TMP_Text>();
        titleText.text = title;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenPausePopup(Action onClose = null)
    {
        PausePopup.Create(onClose);
    }

    private void OpenMenu(string menuName)
    {
        if (string.IsNullOrEmpty(menuName))
            return;
        foreach (var menu in Menus)
            menu.SetActive(menu.name == menuName);
    }

    public GameObject GetMenuByName(string menuName)
    {
        if (string.IsNullOrEmpty(menuName))
            return null;
        foreach (var menu in Menus)
            if(menu.name == menuName)
                return menu;
        return null;
    }
}
