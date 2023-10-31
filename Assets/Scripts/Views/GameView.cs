using Scripts.Managers;
using TMPro;
using UnityEngine;

namespace Scripts.Views
{
    public class GameView : View
    {
        private GameObject _car;
        private DistanceController _distanceController;
        public override void EnterView()
        {
            base.EnterView();
            _car = CarCreator.InstantiateCar(Settings.User.currentSelectedCar);
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
            Destroy(_car);
        }
    }
}