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

        public static void CloseAnim()
        {
            var instance = Utils.FindGameObject("LoadingPopup", UIController.instance.PopupCanvas.gameObject);
            if (instance == null)
                return;

#if UNITY_WEBGL && !UNITY_EDITOR
            // In WebGL, just destroy immediately (DOTween animations don't work reliably)
            Dispose();
#else
            var ms = (DateTime.Now - _startTime).TotalMilliseconds;
            var delayTime = ms < MinimumStayingTimeInMs ? (MinimumStayingTimeInMs - ms) / 1000f : 0f;

            var img = instance.GetComponent<Image>();
            img.color = new Color(0, 0, 0, 1);

            // Use DOTween's delay instead of Task.Delay
            DOVirtual.DelayedCall((float)delayTime, () =>
            {
                img.DOColor(new Color(0, 0, 0, 0), AnimTime).OnComplete(Dispose);
            });
#endif
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
