using System.Collections;
using UnityEngine;
public static class ResourceHelper
{
    public static GameObject InstantiatePrefab(string path, Transform parent)
    {
        GameObject loadedPrefab = Resources.Load<GameObject>(path);
        if (loadedPrefab != null)
            return Object.Instantiate(loadedPrefab, parent );
        else
            Debug.LogError("Prefab not found: " + path);
        return null;
    }

    public static IEnumerator LoadAndInstantiatePrefab(string path, Transform parent, System.Action<GameObject> callback = null)
    {
        ResourceRequest prefabLoadRequest = Resources.LoadAsync<GameObject>(path);

        while (!prefabLoadRequest.isDone)
        {
            // You can update a loading progress bar or perform other tasks here.
            float progress = prefabLoadRequest.progress;
            yield return null; // Yield to the main thread briefly.
        }

        // The prefab is loaded, instantiate it.    
        GameObject prefab = prefabLoadRequest.asset as GameObject;
        var res = Object.Instantiate(prefab, parent);
        callback?.Invoke(res);
    }
}
