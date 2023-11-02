using System.Collections.Generic;
using UnityEngine;
namespace Scripts.Managers
{
    public class ItemController : MonoBehaviour
    {
        #region Singleton
        public static ItemController instance;
        private void Awake()
        {
            instance = this;
        }
        #endregion
        public UserData userData;
        public List<MainMenuTabData> mainMenuTabDatas;
    }
}