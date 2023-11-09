using DG.Tweening;
using Scripts.Managers;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public static class Utils
{
    public static GameObject FindGameObject(string name, GameObject parentOrSelf)
    {
        if (parentOrSelf == null)
            return null;
        if (parentOrSelf.name == name)
            return parentOrSelf;
        var components = parentOrSelf.GetComponentsInChildren<Transform>(true);
        foreach (Transform component in components)
        {
            if (component.gameObject.name == name)
                return component.gameObject;
        }
        return null;
    }

    public static void RunAsync(Action action, float timeoutInSeconds = 0, bool afterEndFrame = false)
    {
        if (action == null)
            return;
        GameManager.Instance.StartCoroutine(RunActionAsap(action, timeoutInSeconds, afterEndFrame));
    }

    private static IEnumerator RunActionAsap(Action action, float timeoutInSeconds = 0, bool afterEndFrame = false)
    {
        if (timeoutInSeconds > 0)
            yield return new WaitForSeconds(timeoutInSeconds);
        else if (!afterEndFrame)
            yield return null;

        if (afterEndFrame)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
        }

        action?.Invoke();
    }

    public static void HideCoin(GameObject coin)
    {
        if (coin.name == "Collected")
            return;
        var sr = coin.GetComponent<SpriteRenderer>();
        var currentPosY = coin.transform.position.y;
        coin.name = "Collected";
        sr.DOColor(new Color(1, 1, 1, 0), 0.5f);
        coin.transform.DOMoveY(currentPosY + 2f, 0.5f).OnComplete(() =>
        {
            UnityEngine.Object.Destroy(coin);
        });
    }

    public static void SetImageSprite(Image image, Sprite sprite)
    {
        image.sprite = sprite;
    }

    public static void AddEventToButton(GameObject obj, EventTriggerType eventTriggerType, Action callbackAction)
    {
        EventTrigger eventTrigger = obj.GetComponent<EventTrigger>() ?? obj.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new()
        {
            eventID = eventTriggerType
        };

        entry.callback.AddListener((eventData) => callbackAction());

        eventTrigger.triggers.Add(entry);
    }
}

enum InputState
{
    None,
    GasPressed,
    BrakePressed,
    GasAndBrakePressed
}
