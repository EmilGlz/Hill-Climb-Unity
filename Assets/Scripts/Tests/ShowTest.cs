using ScriptsPhysicsAndMechanics;
using UnityEngine;

namespace Assets.Scripts.Tests
{
#if UNITY_EDITOR
    public class ShowTest : MonoBehaviour
    {
        #region Singleton
        public static ShowTest instance;
        private void Awake()
        {
            instance = this;
        }
        #endregion
        public GameObject CurrentStage;

        public bool TouchingGround;

        private void Update()
        {
            if(VehicleController.Instance != null)
                TouchingGround = VehicleController.Instance.TouchingGround;
        }
    }
#endif
}
