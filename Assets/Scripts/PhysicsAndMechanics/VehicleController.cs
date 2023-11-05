using Scripts.Managers;
using System.Collections;
using UnityEngine;
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
        private CarData _carData;
        private const float _raycastDistance = 10f;
        private int groundLayer;

        private bool _isCheckingAirTime;
        private int _currentBonusCoins;

        #region Singleton
        public static VehicleController Instance;
        private void Awake()
        {
            Instance = this;
        }
        #endregion

        private void Start()
        {
            _currentBonusCoins = 0;
            _isCheckingAirTime = false;
            groundLayer = LayerMask.NameToLayer("Ground");
        }

        public void Init(CarData data)
        {
            _carData = data;
            _speed = _carData.speed;
            _rotationSpeed = _carData.rotationSpeed;
        }

        private bool IsGrounded()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _raycastDistance, 1 << groundLayer);
            return hit.collider != null;
        }

        IEnumerator CheckAirTime()
        {
            _isCheckingAirTime = true;
            yield return new WaitForSeconds(1f);
            bool isOnAir = !IsGrounded();
            if (!isOnAir)
            {
                _isCheckingAirTime = false;
                UIController.instance.UpdateBonus(0);
                yield break;
            }
            while (isOnAir)
            {
                yield return new WaitForSeconds(0.5f);
                UIController.instance.UpdateBonus(_currentBonusCoins);
                _currentBonusCoins += 500;
                Settings.User.budget += 500;
                Settings.OnPurchase?.Invoke();
                isOnAir = !IsGrounded();
            }
            UIController.instance.UpdateBonus(0);
            _isCheckingAirTime = false;
        }

        private void Update()
        {
            _moveInput = Input.GetAxisRaw("Horizontal");
        }

        private void FixedUpdate()
        {
            var tireTorque = -_moveInput * _speed * Time.fixedDeltaTime;
            if (!IsGrounded())
            {
                if (!_isCheckingAirTime)
                    StartCoroutine(CheckAirTime());
                tireTorque = 0;
            }
            _frontTireRB.AddTorque(tireTorque);
            _backTireRB.AddTorque(tireTorque);
            _carRB.AddTorque(_moveInput * _rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
