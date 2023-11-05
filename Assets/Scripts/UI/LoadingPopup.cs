using DG.Tweening;
using Scripts.Managers;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
namespace Scripts.UI
{
    public class LoadingPopup
    {
        private const string _prefabName = "Prefabs/Popups/LoadingPopup";
        public static float AnimTime => 0.1f; // 0-25% and 75-100% are animation time. Other 50% is stable
        private static int MinimumStayingTimeInMs => 1000;
        private static DateTime _startTime;
        public static void Show(string title = "", Action onLoadingOpened = null, bool openWithAnim = false)
        {
            var instance = Utils.FindGameObject("LoadingPopup", UIController.instance.PopupCanvas.gameObject);
            if (instance != null)
                return;
            instance = ResourceHelper.InstantiatePrefab(_prefabName, UIController.instance.PopupCanvas);
            instance.name = "LoadingPopup";
            _startTime = DateTime.Now;
            OpenAnim(instance, onLoadingOpened, openWithAnim);
        }

        private static void OpenAnim(GameObject instance, Action onLoadingOpened = null, bool openWithAnim = true)
        {
            var img = instance.GetComponent<Image>();
            img.color = new Color(0, 0, 0, 0);
            instance.SetActive(true);
            if (openWithAnim)
                img.DOColor(new Color(0, 0, 0, 1), AnimTime).OnComplete(() =>
                {
                    onLoadingOpened?.Invoke();
                });
            else
            {
                img.color = new Color(0, 0, 0, 1);
            }
        }

        public static async void CloseAnim()
        {
            var instance = Utils.FindGameObject("LoadingPopup", UIController.instance.PopupCanvas.gameObject);
            if (instance == null)
                return;
            var ms = (DateTime.Now - _startTime).Milliseconds;
            if (ms < MinimumStayingTimeInMs)
                await Task.Delay(MinimumStayingTimeInMs - ms);
            var img = instance.GetComponent<Image>();
            img.color = new Color(0, 0, 0, 1);
            img.DOColor(new Color(0, 0, 0, 0), AnimTime).OnComplete(Dispose);
        }

        public static void Dispose()
        {
            var instance = Utils.FindGameObject("LoadingPopup", UIController.instance.PopupCanvas.gameObject);
            if (instance != null)
            {
                UnityEngine.Object.Destroy(instance);
                instance = null;
            }
        }
    }
}
