using System;
using UnityEngine;
using UnityEngine.UI;

public class FuelController : MonoBehaviour
{  
    public static FuelController Instance;

    [SerializeField] private Image _fuelImage;
    [SerializeField] private float _fuelDrainSpeed = 1f;
    [SerializeField] private float _maxFuelAmount = 100f;
    [SerializeField] private Gradient _gradient;

    private float _currentFuelAmount;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _currentFuelAmount = _maxFuelAmount;
        UpdateUI();
    }

    private void Update()
    {
        _currentFuelAmount -= Time.deltaTime * _fuelDrainSpeed;
        UpdateUI();
        if(_currentFuelAmount <= 0)
            GameManager.Instance.GameOver(GameOverCause.FuelEnd);
    }

    private void UpdateUI()
    {
        _fuelImage.fillAmount = (_currentFuelAmount /  _maxFuelAmount);
        _fuelImage.color = _gradient.Evaluate(_fuelImage.fillAmount);
    }
}
