using System;
using UnityEngine;
using UnityEngine.UI;
namespace Scripts.Managers
{
    public class FuelController : IDisposable
    {
        private readonly Image _fuelImage;
        private readonly float _fuelDrainSpeed = 3f;
        private readonly float _maxFuelAmount = 100f;

        private float _currentFuelAmount;

        public FuelController(float currentValue = 100, float maxValue = 100)
        {
            _fuelImage = Utils.FindGameObject("FuelFront", UIController.instance.GetCurrentView().gameObject).GetComponent<Image>();
            _currentFuelAmount = currentValue;
            _maxFuelAmount = maxValue;
            GameManager.Instance.OnUpdate += Update;
            _currentFuelAmount = _maxFuelAmount;
            UpdateUI();
        }

        public void FillUpFuel()
        {
            _currentFuelAmount = _maxFuelAmount;
        }

        private void Update()
        {
            if (Settings.InfiniteFuelOn)
                return;
            _currentFuelAmount -= Time.deltaTime * _fuelDrainSpeed;
            UpdateUI();
            if (_currentFuelAmount <= 0)
                GameManager.Instance.GameOver(GameOverCause.FuelEnd);
        }

        private void UpdateUI()
        {
            _fuelImage.fillAmount = (_currentFuelAmount / _maxFuelAmount);
            _fuelImage.color = UIController.instance.FuelGradient.Evaluate(_fuelImage.fillAmount);
        }
        public void Dispose()
        {
            GameManager.Instance.OnUpdate -= Update;
        }
    }
}