using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
namespace Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public Action OnUpdate;
        public CinemachineVirtualCamera Camera;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            Settings.User = ItemController.instance.userData;
            if (Settings.User == null) 
            {
                Settings.User = Settings.ResettedUser();
                Settings.SaveUserData();
            }
        }
        private void Update()
        {
            OnUpdate?.Invoke();
        }
        public void GameOver(GameOverCause cause = GameOverCause.None, int delay = 1)
        {
            var title = "";
            switch (cause)
            {
                case GameOverCause.None:
                    title = "Game Over!";
                    break;
                case GameOverCause.HeadCrack:
                    title = "Head cracked!";
                    break;
                case GameOverCause.FuelEnd:
                    title = "Fuel finished!";
                    break;
            }
            StartCoroutine(GameOverActions());
            IEnumerator GameOverActions()
            {
                yield return new WaitForSeconds(delay);
                UIController.instance.OpenGameOverMenu(title);
                Time.timeScale = 0f;
            }
        }
        public void PauseGame()
        {
            UIController.instance.OpenPausePopup(() =>
            {
                Time.timeScale = 1f;
            },
            () =>
            {
                Time.timeScale = 0f;
            });
        }
    }

    public enum GameOverCause
    {
        None,
        HeadCrack,
        FuelEnd
    }
}