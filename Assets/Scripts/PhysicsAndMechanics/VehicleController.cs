using Scripts.Managers;
using System;
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
        private Action<GameObject> _onGroundStartReached;
        private Action<GameObject> _onGroundFinishReached;
        private CarData _carData;
        private const float _raycastDistance = 10f;
        private int groundLayer;

        private bool _isCheckingAirTime;
        private int _currentBonusCoins;

        private bool _touchingGround;
        public bool TouchingGround { 
            get => _touchingGround;
            set
            {
                _touchingGround = value;
                if (_touchingGround)
                    mustStopAirTime = true;
            }
        }
        private bool mustStopAirTime = false;

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

        public void Init(CarData data, Action<GameObject> onGroundStartReached, Action<GameObject> onGroundFinishReached)
        {
            _onGroundStartReached = onGroundStartReached;
            _onGroundFinishReached = onGroundFinishReached;
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
                _currentBonusCoins += 500;
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
            _moveInput = Input.GetAxisRaw("Horizontal");
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
                Utils.HideCoin(collision.gameObject);
            }
            else if (collision.gameObject.CompareTag("GroundStart"))
                _onGroundStartReached.Invoke(collision.gameObject);
            else if (collision.gameObject.CompareTag("GroundEnd"))
                _onGroundFinishReached.Invoke(collision.gameObject);
        }
    }
}
