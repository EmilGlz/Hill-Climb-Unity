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

        public static VehicleController Instance;
        private void Awake()
        {
            Instance = this;
        }

        public void Init(CarData data)
        {
            _carData = data;
            _speed = _carData.speed;
            _rotationSpeed = _carData.rotationSpeed;
        }

        private void Update()
        {
            _moveInput = Input.GetAxisRaw("Horizontal");
        }

        private void FixedUpdate()
        {
            _frontTireRB.AddTorque(-_moveInput * _speed * Time.fixedDeltaTime);
            _backTireRB.AddTorque(-_moveInput * _speed * Time.fixedDeltaTime);
            _carRB.AddTorque(_moveInput * _rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
