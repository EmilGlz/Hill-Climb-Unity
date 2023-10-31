using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class ExtensionMethods
{
    public static Transform RemoveAllChilds(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        return transform;
    }

    public static string ToCamelCase(this string s, CultureInfo culture = null)
    {
        if (string.IsNullOrEmpty(s))
            return s;
        return s[0].ToString().ToUpper(culture ?? CultureInfo.CurrentCulture) + s.Substring(1).ToLower(culture ?? CultureInfo.CurrentCulture);
    }

    public static string ToCapitalization(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return s;
        return s[0].ToString().ToUpper() + s.Substring(1);
    }

    public static bool IsDestroyed(this GameObject obj)
    {
        return obj == null;
    }

    public static bool IsDestroyed(this RectTransform obj)
    {
        return obj == null;
    }

    public static bool IsDestroyed(this MonoBehaviour obj)
    {
        return obj == null;
    }

    public static float GetPosX(this RectTransform rect)
    {
        return rect.anchoredPosition.x;
    }

    public static void SetPosX(this RectTransform rect, float value)
    {
        rect.anchoredPosition = new Vector2(value, rect.anchoredPosition.y);
    }

    public static float GetPosY(this RectTransform rect)
    {
        return rect.anchoredPosition.y;
    }

    public static void SetPosY(this RectTransform rect, float value)
    {
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, value);
    }

    public static float GetWidth(this RectTransform rect)
    {
        return rect.sizeDelta.x;
    }

    public static void SetWidth(this RectTransform rect, float value)
    {
        rect.sizeDelta = new Vector2(value, rect.sizeDelta.y);
    }

    public static float GetHeight(this RectTransform rect)
    {
        return rect.sizeDelta.y;
    }

    public static void SetHeight(this RectTransform rect, float value)
    {
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, value);
    }

    public static float GetLeft(this RectTransform rect)
    {
        return rect.offsetMin.x;
    }

    public static void SetLeft(this RectTransform rect, float value)
    {
        rect.offsetMin = new Vector2(value, rect.offsetMin.y);
    }

    public static float GetBottom(this RectTransform rect)
    {
        return rect.offsetMin.y;
    }

    public static void SetBottom(this RectTransform rect, float value)
    {
        rect.offsetMin = new Vector2(rect.offsetMin.x, value);
    }

    public static float GetRight(this RectTransform rect)
    {
        return -rect.offsetMax.x;
    }

    public static void SetRight(this RectTransform rect, float value)
    {
        rect.offsetMax = new Vector2(-value, rect.offsetMax.y);
    }

    public static float GetTop(this RectTransform rect)
    {
        return -rect.offsetMax.y;
    }

    public static void SetTop(this RectTransform rect, float value)
    {
        rect.offsetMax = new Vector2(rect.offsetMax.x, -value);
    }

    public static Rect GetScreenRect(this RectTransform transform)
    {
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        float x = transform.position.x + transform.anchoredPosition.x;
        float y = Screen.height - transform.position.y - transform.anchoredPosition.y;

        return new Rect(x, y, size.x, size.y);
    }

    public static Vector3 Reverse(this Vector3 v)
    {
        return new Vector3(1f / v.x, 1f / v.y, 1f / v.z);
    }

    public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        TKey maxValue = default(TKey);
        TSource maxElement = default(TSource);
        bool found = false;

        foreach (var element in source)
        {
            if (!found || Comparer<TKey>.Default.Compare(selector(element), maxValue) > 0)
            {
                maxValue = selector(element);
                maxElement = element;
                found = true;
            }
        }

        if (!found)
            throw new InvalidOperationException("Sequence contains no elements.");

        return maxElement;
    }

    //public static void Shuffle<T>(this IList<T> list)
    //{
    //    int n = list.Count;
    //    while (n > 1)
    //    {
    //        n--;
    //        int k = Settings.Rand.Next(n + 1);
    //        T value = list[k];
    //        list[k] = list[n];
    //        list[n] = value;
    //    }
    //}

}
