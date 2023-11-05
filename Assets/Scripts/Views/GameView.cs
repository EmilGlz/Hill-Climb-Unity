using Scripts.Managers;
using Scripts.UI;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Scripts.Views
{
    public class GameView : View
    {
        private GameObject _car;
        private GameObject _stage;
        private DistanceController _distanceController;
        private TMP_Text _bonusText;
        private TMP_Text _budgetText;
        private GameOverView _gameOverView;
        public override async Task EnterView()
        {
            LoadingPopup.Show();
            await base.EnterView();
            _car = LevelUtils.InstantiateCar(Settings.User.currentSelectedCar);
            _stage = LevelUtils.InstantiateStage(Settings.User.currentSelectedStage);
            GameManager.Instance.VirtualCamera.Follow = _car.transform;

            _bonusText = Utils.FindGameObject("BonusText", gameObject).GetComponent<TMP_Text>();
            _bonusText.gameObject.SetActive(true);
            _bonusText.text = "";

            _budgetText = Utils.FindGameObject("CoinText", gameObject).GetComponent<TMP_Text>();
            UpdateBudgetUI();

            var distanceText = Utils.FindGameObject("DistanceText", gameObject).GetComponent<TMP_Text>();
            _distanceController = new DistanceController(distanceText, _car.transform);
            Settings.OnPurchase += UpdateBudgetUI;

            _gameOverView = Utils.FindGameObject("GameOverView", gameObject).GetComponent<GameOverView>();
            _gameOverView.ExitView();

            LoadingPopup.CloseAnim();
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
        }

        public void UpdateBudgetUI()
        {
            _budgetText.text = Settings.User.budget.ToString("#,##0");
        }

        public void OpenGameOverMenu()
        {
            _gameOverView.EnterView();
        }

        public override void ExitView()
        {
            Settings.OnPurchase -= UpdateBudgetUI;
            base.ExitView();

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
            if (_stage != null)
            {
                Destroy(_stage);
                _stage = null;
            }
            gameObject.SetActive(false);
        }
    }
}