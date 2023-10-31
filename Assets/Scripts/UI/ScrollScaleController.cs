using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Scripts.UI
{
    public class ScrollScaleController : MonoBehaviour
    {
        [SerializeField] List<RectTransform> _items;
        [SerializeField] RectTransform _content;
        private const float _scrolltime = 0.1f;
        private bool _isDragging;

        public static ScrollScaleController instance;
        private void Awake()
        {
            instance = this;
        }

        public void InitItems()
        {
            _items = new List<RectTransform>();
            for (int i = 0; i < _content.childCount; i++)
            {
                RectTransform child = _content.GetChild(i).GetComponent<RectTransform>();
                if (child != null && child.gameObject.activeInHierarchy)
                    _items.Add(child);
            }
            var lg = _content.GetComponent<HorizontalLayoutGroup>();
            if (lg != null)
                lg.padding.left = lg.padding.right = (int)(Device.Width / 2f - _items[0].GetWidth() / 2f);
            UpdateItemsScale();
            // TODO Check, scale must be 1, not 0
            // And Make scrollview start from the left side
        }

        private bool IsOutsideTheScreen(RectTransform item)
        {
            bool isOutsideFromLeft = (item.GetPosX() + _content.GetPosX() + item.GetWidth() / 2f) < 0;
            bool isOutsideFromRight = (Mathf.Abs(_content.GetPosX()) + Device.Width) < (item.GetPosX() - item.GetWidth() / 2f);
            return isOutsideFromLeft || isOutsideFromRight;
        }

        private int DistanceFromMiddleOfScreen(RectTransform item)
        {
            var res = (int)(Mathf.Abs(_content.GetPosX()) + Device.Width / 2f - item.GetPosX());
            if (_content.GetPosX() > 0)
                res = -res;
            return res;
        }

        public void OnBeginDrag()
        {
            _isDragging = true;
        }

        public void OnDrag()
        {
            UpdateItemsScale();
        }

        private void UpdateItemsScale()
        {
            foreach (var item in _items)
            {
                bool isOutsideScreen = IsOutsideTheScreen(item);
                var distanceFromMiddle = DistanceFromMiddleOfScreen(item);
                var deviceWidth = Device.Width;
                if (!isOutsideScreen)
                {
                    var halfDeviceWidth = deviceWidth / 2f;
                    var resScale = Vector3.one * 1 / (halfDeviceWidth / Mathf.Abs(distanceFromMiddle));
                    if (resScale.x > 1)
                        resScale = Vector3.one;
                    resScale = -(resScale - Vector3.one);
                    item.transform.localScale = resScale;
                }
                else
                {
                    item.transform.localScale = Vector3.one * 0.001f;
                }
            }
        }

        public void OnEndDrag()
        {
            _isDragging = false;
            StartCoroutine(ScrollToClosestItem(.1f));
        }

        IEnumerator ScrollToClosestItem(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (_isDragging)
                yield break;
            var target = GetBiggestItem();
            var targetContentPosition = target.GetPosX() - _items[0].GetPosX();
            Vector2 startPosition = _content.anchoredPosition;
            float elapsedTime = 0f;
            Vector2 tempVector;
            while (elapsedTime < _scrolltime)
            {
                tempVector = Vector2.Lerp(startPosition, Vector2.left * targetContentPosition, elapsedTime / _scrolltime);
                elapsedTime += Time.deltaTime;
                _content.SetPosX(tempVector.x);
                UpdateItemsScale();
                yield return null;
            }
            _content.SetPosX(-targetContentPosition);
            UpdateItemsScale();
        }

        private RectTransform GetBiggestItem()
        {
            RectTransform largestLocalScaleTransform = _items[0];
            float largestScaleMagnitude = largestLocalScaleTransform.localScale.magnitude;

            for (int i = 1; i < _items.Count; i++)
            {
                float currentMagnitude = _items[i].localScale.magnitude;
                if (currentMagnitude > largestScaleMagnitude)
                {
                    largestScaleMagnitude = currentMagnitude;
                    largestLocalScaleTransform = _items[i];
                }
            }
            return largestLocalScaleTransform;
        }
    }
}