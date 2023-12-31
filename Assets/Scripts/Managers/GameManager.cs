using Cinemachine;
using Scripts.Views;
using System;
using System.Collections;
using UnityEngine;
namespace Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public Action OnUpdate;
        public CinemachineVirtualCamera VirtualCamera;
        [HideInInspector] public Camera MainCamera;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            MainCamera = Camera.main;
            Settings.User = ItemController.instance.userData;
            if (Settings.User.currentSelectedStage == null || Settings.User.currentSelectedCar == null)
            {
                Settings.User.currentSelectedStage = Settings.User.stages[0];
                Settings.User.currentSelectedCar = Settings.User.ownedCars[0];
            }
            UIController.instance.EnterView<MainMenuView>();
            Settings.ShowPedals = Device.IsMobile ? true : Device.IsEditor ? Settings.ShowPedalsInEditor : true;
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