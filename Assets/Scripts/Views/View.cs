using System.Threading.Tasks;
using UnityEngine;
namespace Scripts.Views
{
    public class View : MonoBehaviour
    {
        private void Start()
        {
            gameObject.name = GetViewName();
        }

        public virtual Task EnterView()
        {
            return Task.CompletedTask;
        }

        public virtual void ExitView()
        {

        }

        public virtual void PauseResumeView(bool resume)
        {
            gameObject.SetActive(resume);
        }

        public string GetViewName()
        {
            return GetType().Name;
        }
    }
}