using DG.Tweening;
using Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public static List<GameObject> FindGameObjects(string name, GameObject parentOrSelf)
    {
        var res = new List<GameObject>();
        if (parentOrSelf == null)
            return null;
        if (parentOrSelf.name == name)
            res.Add(parentOrSelf);
        var components = parentOrSelf.GetComponentsInChildren<Transform>(true);
        foreach (Transform component in components)
        {
            if (component.gameObject.name == name)
                res.Add(component.gameObject);
        }
        return res;
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
        var nameBefore = coin.name;
        if (coin.name == "Collected")
            return;
        var collider = coin.GetComponent<CircleCollider2D>();
        if(collider != null)
            collider.enabled = false;
        var startPos = coin.transform.position;
        var sr = coin.GetComponent<SpriteRenderer>();
        var currentPosY = coin.transform.position.y;
        coin.name = "Collected";
        sr.DOColor(new Color(1, 1, 1, 0), 0.5f);
        coin.transform.DOMoveY(currentPosY + 2f, 0.5f).OnComplete(() =>
        {
            coin.SetActive(false);
            coin.transform.position = startPos;
            coin.name = nameBefore;
        });
    }

    public static void ShowAllCollectables(GameObject stage)
    {
        var fuels = FindGameObjects("Fuel", stage);
        foreach (var fuel in fuels)
        {
            ShowCollectable(fuel, "Fuel");
            var collider = fuel.GetComponent<BoxCollider2D>();
            if (collider != null)
                collider.enabled = true;
        }
        var coinsParent = FindGameObject("Coins", stage);
        foreach (Transform child in coinsParent.transform)
        {
            ShowCollectable(child.gameObject, "Coin");
            var collider = child.GetComponent<CircleCollider2D>();
            if (collider != null)
                collider.enabled = true;
        }
    }

    private static void ShowCollectable(GameObject obj, string name)
    {
        obj.SetActive(true);
        obj.GetComponent<SpriteRenderer>().color = Color.white;
        obj.name = name;
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
