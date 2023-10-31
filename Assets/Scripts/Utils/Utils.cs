using System;
using System.Collections;
using UnityEngine;
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
}
