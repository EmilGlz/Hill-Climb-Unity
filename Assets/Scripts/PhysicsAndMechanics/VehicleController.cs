using Scripts.Managers;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace ScriptsPhysicsAndMechanics
{
    public class VehicleController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _frontTireRB;
        [SerializeField] private Rigidbody2D _backTireRB;
        [SerializeField] private Rigidbody2D _carRB;
        [SerializeField] private float _speed = 150f;
        [SerializeField] private float _rotationSpeed = 300f;

        private float _moveInput;
        private Action<GameObject> _onGroundStartReached;
        private Action<GameObject> _onGroundFinishReached;
        private CarData _carData;
        private int _currentMatchCoins;

        private bool _isCheckingAirTime;
        private bool _touchingGround;
        public bool TouchingGround
        {
            get => _touchingGround;
            set
            {
                _touchingGround = value;
                if (_touchingGround)
                    mustStopAirTime = true;
            }
        }
        private bool mustStopAirTime = false;

        private GameObject _gasButton;
        private GameObject _brakeButton;
        private bool _gasPressed;
        private bool _brakePressed;

        private InputState _inputState;

        #region Singleton
        public static VehicleController Instance;
        private void Awake()
        {
            Instance = this;
        }
        #endregion

        private void Start()
        {
            _currentMatchCoins = 0;
            _isCheckingAirTime = false;
        }

        public void Init(CarData data, Action<GameObject> onGroundStartReached, Action<GameObject> onGroundFinishReached, GameObject gasButton = null, GameObject brakeButton = null)
        {
            _gasButton = gasButton;
            _brakeButton = brakeButton;
            _onGroundStartReached = onGroundStartReached;
            _onGroundFinishReached = onGroundFinishReached;
            _carData = data;
            _speed = _carData.speed;
            _rotationSpeed = _carData.rotationSpeed;
            if (_gasButton == null)
                return;
            if (_brakeButton == null)
                return;
            if (Settings.ShowPedals)
            {
                _gasButton.SetActive(true);
                Utils.AddEventToButton(_gasButton, UnityEngine.EventSystems.EventTriggerType.PointerDown, () =>
                {
                    _gasPressed = true;
                    UIController.instance.PedalPressed(_gasButton, true);
                });
                Utils.AddEventToButton(_gasButton, UnityEngine.EventSystems.EventTriggerType.PointerUp, () =>
                {
                    _gasPressed = false;
                    UIController.instance.PedalNotPressed(_gasButton, true);
                });

                _brakeButton.SetActive(true);
                Utils.AddEventToButton(_brakeButton, UnityEngine.EventSystems.EventTriggerType.PointerDown, () =>
                {
                    _brakePressed = true;
                    UIController.instance.PedalPressed(_brakeButton, false);
                });
                Utils.AddEventToButton(_brakeButton, UnityEngine.EventSystems.EventTriggerType.PointerUp, () =>
                {
                    _brakePressed = false;
                    UIController.instance.PedalNotPressed(_brakeButton, false);
                });
            }
            else
            {
                _brakeButton.SetActive(false);
                _gasButton.SetActive(false);
            }
        }

        IEnumerator CheckAirTime()
        {
            var currentAirBonus = 0;
            _isCheckingAirTime = true;
            yield return new WaitForSeconds(1f);
            if (TouchingGround)
            {
                _isCheckingAirTime = false;
                UIController.instance.UpdateBonus(0);
                yield break;
            }
            mustStopAirTime = false;
            while (!TouchingGround && !mustStopAirTime)
            {
                yield return new WaitForSeconds(0.5f);
                _currentMatchCoins += 500;
                currentAirBonus += 500;
                Settings.User.budget += 500;
                UIController.instance.UpdateBonus(currentAirBonus);
                Settings.OnPurchase?.Invoke();
            }
            UIController.instance.UpdateBonus(0);
            _isCheckingAirTime = false;
        }

        private void Update()
        {
            if (!Settings.ShowPedals)
                _moveInput = Input.GetAxisRaw("Horizontal");

            else
            {
                if (_gasPressed && _brakePressed)
                    _inputState = InputState.GasAndBrakePressed;
                else if (_gasPressed)
                    _inputState = InputState.GasPressed;
                else if (_brakePressed)
                    _inputState = InputState.BrakePressed;
                else
                    _inputState = InputState.None;
                _moveInput = _inputState switch
                {
                    InputState.None => 0,
                    InputState.GasPressed => 1,
                    InputState.BrakePressed => -1,
                    InputState.GasAndBrakePressed => 0,
                    _ => (float)0,
                };
            }
        }

        private void FixedUpdate()
        {
            var tireTorque = -_moveInput * _speed * Time.fixedDeltaTime;
            if (!TouchingGround)
            {
                if (!_isCheckingAirTime)
                    StartCoroutine(CheckAirTime());
                tireTorque = 0;
            }
            _frontTireRB.AddTorque(tireTorque);
            _backTireRB.AddTorque(tireTorque);
            _carRB.AddTorque(_moveInput * _rotationSpeed * Time.fixedDeltaTime);
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject != null && collision.gameObject.CompareTag("Coin500"))
            {
                Settings.User.budget += 500;
                Settings.OnPurchase?.Invoke();
                Utils.HideCoin(collision.gameObject);
            }
            else if (collision.gameObject != null && collision.gameObject.CompareTag("Fuel"))
            {
                Settings.OnFuelCollected?.Invoke();
                Utils.HideCoin(collision.gameObject);
            }
            else if (collision.gameObject.CompareTag("GroundStart"))
                _onGroundStartReached.Invoke(collision.gameObject);
            else if (collision.gameObject.CompareTag("GroundEnd"))
                _onGroundFinishReached.Invoke(collision.gameObject);
        }
    }
}
