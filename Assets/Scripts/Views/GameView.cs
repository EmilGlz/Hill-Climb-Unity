using Assets.Scripts.Tests;
using Scripts.Managers;
using Scripts.UI;
using ScriptsPhysicsAndMechanics;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Views
{
    public class GameView : View
    {
        private GameObject _car;
        private GameObject _currentStage;
        private DistanceController _distanceController;
        private FuelController _fuelController;
        private TMP_Text _bonusText;
        private TMP_Text _budgetText;
        private GameOverView _gameOverView;
        private List<GameObject> _stages;
        private int _budgetBefore;
        private int CurrentCollectedCoins => Settings.User.budget - _budgetBefore;

        private GameObject CurrentStage
        {
            get { return _currentStage; }
            set
            {
                ShowTest.instance.CurrentStage = value;
                _currentStage = value;
            }
        }

        public override async Task EnterView()
        {
            LoadingPopup.Show();
            _budgetBefore = Settings.User.budget;
            _fuelController = new FuelController();
            await base.EnterView();

            var gasButton = Utils.FindGameObject("GasButton", gameObject);
            var brakeButton = Utils.FindGameObject("BrakeButton", gameObject);

            _car = LevelUtils.InstantiateCar(Settings.User.currentSelectedCar);
            var carController = _car.GetComponent<VehicleController>();
            carController.Init(Settings.User.currentSelectedCar, OnGroundStartReached, OnGroundEndReached, gasButton, brakeButton);
            var currentGravity = Settings.User.currentSelectedStage.gravityScale;
            _car.GetComponent<Rigidbody2D>().gravityScale = currentGravity;

            _stages = new List<GameObject>();
            for (int i = 0; i < 2; i++)
            {
                var newStage = LevelUtils.InstantiateStage(Settings.User.currentSelectedStage);
                newStage.name = "Stage " + i;
                _stages.Add(newStage);
            }
            CurrentStage = _stages[0];
            LevelUtils.UpdateSkyBackground(Settings.User.currentSelectedStage);
            _stages[0].transform.position = Vector3.zero;
            var endPos = Utils.FindGameObject("GroundEnd", CurrentStage).transform.position;
            var startPos = Utils.FindGameObject("GroundStart", CurrentStage).transform.position;
            _stages[1].transform.position = _stages[0].transform.position + Vector3.left * LevelUtils.GetWidth(_stages[1]) + Vector3.up * (startPos.y - endPos.y);

            GameManager.Instance.VirtualCamera.Follow = _car.transform;

            _bonusText = Utils.FindGameObject("BonusText", gameObject).GetComponent<TMP_Text>();
            _bonusText.gameObject.SetActive(true);
            _bonusText.text = "";

            _budgetText = Utils.FindGameObject("CoinText", gameObject).GetComponent<TMP_Text>();
            UpdateBudgetUI();

            var distanceText = Utils.FindGameObject("DistanceText", gameObject).GetComponent<TMP_Text>();
            _distanceController = new DistanceController(distanceText, _car.transform);

            Settings.OnFuelCollected += FillUpFuel;
            Settings.OnCoinCollected += OnCoinCollected;

            _gameOverView = Utils.FindGameObject("GameOverView", gameObject).GetComponent<GameOverView>();
            _gameOverView.ExitView();

            LoadingPopup.CloseAnim();
        }

        private void FillUpFuel()
        {
            _fuelController.FillUpFuel();
        }

        private GameObject GetNotCurrentStage()
        {
            GetCurrentStage();
            return CurrentStage == _stages[0] ? _stages[1] : _stages[0];
        }

        private void GetCurrentStage()
        {
            foreach (var stage in _stages)
            {
                var startPos = Utils.FindGameObject("GroundStart", stage);
                var endPos = Utils.FindGameObject("GroundEnd", stage);
                if (_car.transform.position.x < endPos.transform.position.x && _car.transform.position.x > startPos.transform.position.x)
                    CurrentStage = stage;
            }
        }

        private void OnGroundEndReached(GameObject collider)
        {
            var movingGround = GetNotCurrentStage();
            var endPos = Utils.FindGameObject("GroundEnd", CurrentStage).transform.position;
            var startPos = Utils.FindGameObject("GroundStart", CurrentStage).transform.position;
            movingGround.transform.position = CurrentStage.transform.position + LevelUtils.GetWidth(CurrentStage) * Vector3.right + Vector3.up * (endPos.y - startPos.y);
            GetCurrentStage();
            Utils.ShowAllCollectables(movingGround);
        }

        private void OnGroundStartReached(GameObject collider) // when user goes background
        {
            var movingGround = GetNotCurrentStage();
            var endPos = Utils.FindGameObject("GroundEnd", CurrentStage).transform.position;
            var startPos = Utils.FindGameObject("GroundStart", CurrentStage).transform.position;
            movingGround.transform.position = CurrentStage.transform.position - LevelUtils.GetWidth(CurrentStage) * Vector3.right + Vector3.up * (endPos.y - startPos.y);
            GetCurrentStage();
        }

        private void OnCoinCollected(int coins)
        {
            Settings.User.budget += coins;
            UpdateBudgetUI();
        }

        public void UpdateBonus(int bonus)
        {
            _bonusText.gameObject.SetActive(true);
            if (_bonusText == null)
                return;
            if (bonus != 0)
                _bonusText.text = "AIR TIME" + "\n+ " + bonus;
            else
                _bonusText.text = "";
            UpdateBudgetUI();
        }

        public void UpdateBudgetUI()
        {
            _budgetText.text = Settings.User.budget.ToString("#,##0");
        }

        public void OpenGameOverMenu()
        {
            _gameOverView.EnterView();
            _gameOverView.Init((int)_distanceController.Distance, CurrentCollectedCoins);
        }

        public override void ExitView()
        {
            Settings.OnCoinCollected -= OnCoinCollected;
            Settings.OnFuelCollected -= FillUpFuel;
            base.ExitView();
            if (_fuelController != null)
            {
                _fuelController.Dispose();
                _fuelController = null;
            }
            if (_gameOverView != null)
            {
                _gameOverView.ExitView();
                _gameOverView = null;
            }
            if (_distanceController != null)
            {
                _distanceController.Dispose();
                _distanceController = null;
            }
            if (_car != null)
            {
                Destroy(_car);
                _car = null;
            }
            if (_stages != null)
            {

                foreach (var item in _stages)
                {
                    if (item != null)
                        Destroy(item);
                }
            }
            CurrentStage = null;
            gameObject.SetActive(false);
        }
    }
}