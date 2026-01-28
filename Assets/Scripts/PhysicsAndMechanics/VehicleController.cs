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

        private bool _isCheckingAirTime;
        private bool _touchingGround;
        public bool TouchingGround
        {
            get => _touchingGround;
            set
            {
                _touchingGround = value;
                if (_touchingGround)
                     _mustStopAirTime = true;
            }
        }

        private GameObject _gasButton;
        private GameObject _brakeButton;
        private bool _gasPressed;
        private bool _brakePressed;

        private InputState _inputState;

        #region Singleton
        public static VehicleController Instance;
        private bool _mustStopAirTime;

        private void Awake()
        {
            Instance = this;
        }
        #endregion

        private void Start()
        {
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

            // Always show the pedal buttons
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

        IEnumerator CheckAirTime()
        {
            var currentAirBonus = 0;
            _isCheckingAirTime = true;

            // Wait 1 second before starting air time bonus
            yield return new WaitForSeconds(1f);

            // Check if landed during the wait
            if (TouchingGround || _mustStopAirTime)
            {
                _isCheckingAirTime = false;
                UIController.instance.UpdateBonus(0);
                yield break;
            }

            _mustStopAirTime = false;

            // Give bonus every 1.5 seconds while in air
            while (!TouchingGround && !_mustStopAirTime)
            {
                yield return new WaitForSeconds(1.5f);

                // Double-check still in air before giving bonus
                if (TouchingGround || _mustStopAirTime)
                    break;

                Settings.User.budget += 500;
                currentAirBonus += 500;
                UIController.instance.UpdateBonus(currentAirBonus);
            }

            UIController.instance.UpdateBonus(0);
            _isCheckingAirTime = false;
        }

        private void Update()
        {
            // Check for keyboard input (Right arrow = Gas, Left arrow = Brake)
            bool keyboardGas = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
            bool keyboardBrake = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);

            // Handle keyboard key down/up events to update UI
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if (_gasButton != null)
                    UIController.instance.PedalPressed(_gasButton, true);
            }
            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
            {
                if (_gasButton != null)
                    UIController.instance.PedalNotPressed(_gasButton, true);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if (_brakeButton != null)
                    UIController.instance.PedalPressed(_brakeButton, false);
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
            {
                if (_brakeButton != null)
                    UIController.instance.PedalNotPressed(_brakeButton, false);
            }

            // Combine keyboard and button input
            bool gasActive = _gasPressed || keyboardGas;
            bool brakeActive = _brakePressed || keyboardBrake;

            // Determine input state
            if (gasActive && brakeActive)
                _inputState = InputState.GasAndBrakePressed;
            else if (gasActive)
                _inputState = InputState.GasPressed;
            else if (brakeActive)
                _inputState = InputState.BrakePressed;
            else
                _inputState = InputState.None;

            // Set move input based on state
            _moveInput = _inputState switch
            {
                InputState.None => 0,
                InputState.GasPressed => 1,
                InputState.BrakePressed => -1,
                InputState.GasAndBrakePressed => 0,
                _ => (float)0,
            };
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
                // Check if already collected to prevent double-counting from multiple colliders
                if (collision.gameObject.name == "Collected")
                    return;
                collision.gameObject.name = "Collected"; // Mark immediately before invoking
                Settings.OnCoinCollected?.Invoke(500);
                Utils.HideCoin(collision.gameObject);
            }
            else if (collision.gameObject != null && collision.gameObject.CompareTag("Fuel"))
            {
                if (collision.gameObject.name == "Collected")
                    return;
                collision.gameObject.name = "Collected";
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
