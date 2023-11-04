using Scripts.Managers;
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
        public override async Task EnterView()
        {
            await base.EnterView();
            _car = LevelUtils.InstantiateCar(Settings.User.currentSelectedCar);
            _stage = LevelUtils.InstantiateStage(Settings.User.currentSelectedStage);
            GameManager.Instance.Camera.Follow = _car.transform;

            var text = Utils.FindGameObject("DistanceText", gameObject).GetComponent<TMP_Text>();
            _distanceController = new DistanceController(text, transform);
        }

        public override void ExitView()
        {
            base.ExitView();
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
        }
    }
}