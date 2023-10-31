using UnityEngine;
namespace Scripts.Views
{
    public class View : MonoBehaviour
    {
        public virtual void EnterView()
        {

        }
        public virtual void ExitView()
        {

        }
        public string GetViewName()
        {
            return GetType().Name;
        }
    }
}