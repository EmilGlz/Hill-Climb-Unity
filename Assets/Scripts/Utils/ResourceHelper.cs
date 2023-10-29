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
}
