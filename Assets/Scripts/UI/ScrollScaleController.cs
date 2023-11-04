using Scripts.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Scripts.UI
{
    public class ScrollScaleController : MonoBehaviour, IDisposable
    {
        private RectTransform _content;
        private List<Item> _items;
        private const float _scrolltime = 0.1f;
        private bool _isDragging;

        public void InitItems(List<Item> items)
        {
            _content = Utils.FindGameObject("Content", gameObject).GetComponent<RectTransform>();
            _items = items;
            //_items = new List<RectTransform>();
            //for (int i = 0; i < items.Count; i++)
            //{
            //    _items.Add(items[i].Rect);
            //}
            var lg = _content.GetComponent<HorizontalLayoutGroup>();
            if (lg != null)
                lg.padding.left = lg.padding.right = (int)(Device.Width / 2f - _items[0].Rect.GetWidth() / 2f);
            UpdateItemsScale();
            // TODO Check, scale must be 1, not 0
            // And Make scrollview start from the left side
        }

        public static bool InMiddle(Item item)
        {
            return item.Instance.transform.localScale.x > 0.9f;
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
                var rect = item.Rect;
                var obj = item.Instance;
                bool isOutsideScreen = IsOutsideTheScreen(rect);
                var distanceFromMiddle = DistanceFromMiddleOfScreen(rect);
                var deviceWidth = Device.Width;
                if (!isOutsideScreen)
                {
                    var halfDeviceWidth = deviceWidth / 2f;
                    var resScale = Vector3.one * 1 / (halfDeviceWidth / Mathf.Abs(distanceFromMiddle));
                    resScale = -(resScale - Vector3.one);

                    if (item is IScaler scaler)
                        scaler.UpdateScale(resScale.x);

                    if (item is IDarker darker)
                        darker.UpdateOverlay(1 - resScale.x);
                }
                else
                {
                    obj.transform.localScale = Vector3.one * 0.001f;
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
            var targetContentPosition = target.GetPosX() - _items[0].Rect.GetPosX();
            Vector2 startPosition = _content.anchoredPosition;
            float elapsedTime = 0f;
            Vector2 tempVector;
            while (elapsedTime < _scrolltime)
            {
                tempVector = Vector2.Lerp(startPosition, Vector2.left * targetContentPosition, elapsedTime / _scrolltime);
                elapsedTime += Time.deltaTime;
                _content.SetPosX(tempVector.x);
                UpdateItemsScale();
                if (_isDragging)
                    yield break;
                yield return null;
            }
            _content.SetPosX(-targetContentPosition);
            UpdateItemsScale();
        }

        public IEnumerator ScrollTo(Item item)
        {
            if (_isDragging)
                yield break;
            var target = item.Rect;
            var targetContentPosition = target.GetPosX() - _items[0].Rect.GetPosX();
            Vector2 startPosition = _content.anchoredPosition;
            float elapsedTime = 0f;
            Vector2 tempVector;
            while (elapsedTime < _scrolltime)
            {
                tempVector = Vector2.Lerp(startPosition, Vector2.left * targetContentPosition, elapsedTime / _scrolltime);
                elapsedTime += Time.deltaTime;
                _content.SetPosX(tempVector.x);
                UpdateItemsScale();
                if (_isDragging)
                    yield break;
                yield return null;
            }
            _content.SetPosX(-targetContentPosition);
            UpdateItemsScale();
        }

        private RectTransform GetBiggestItem()
        {
            if(_items == null || _items.Count == 0)
                return null;
            RectTransform largestLocalScaleTransform = _items[0].Rect;
            float largestScaleMagnitude = largestLocalScaleTransform.localScale.magnitude;

            for (int i = 1; i < _items.Count; i++)
            {
                float currentMagnitude = _items[i].Rect.localScale.magnitude;
                if (currentMagnitude > largestScaleMagnitude)
                {
                    largestScaleMagnitude = currentMagnitude;
                    largestLocalScaleTransform = _items[i].Rect;
                }
            }
            return largestLocalScaleTransform;
        }

        public void Dispose()
        {
            foreach (var item in _items)
            {
                item?.Dispose();
            }
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }
}