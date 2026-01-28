using System.Collections;
using System.Threading.Tasks;
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

    public static async Task<GameObject> InstantiatePrefabAsync(string prefabPath, Transform parent)
    {
        // Use synchronous loading for WebGL to avoid async/await issues
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            return InstantiatePrefab(prefabPath, parent);
        }

        ResourceRequest request = Resources.LoadAsync(prefabPath, typeof(GameObject));

        while (!request.isDone)
            await Task.Delay(1);

        GameObject prefab = request.asset as GameObject;

        if (prefab == null)
        {
            Debug.LogError("Prefab not found at path: " + prefabPath);
            return null;
        }

        GameObject instantiatedObject = GameObject.Instantiate(prefab, parent);
        return instantiatedObject;
    }
    public static async Task<Sprite> LoadSpriteAsync(string spritePath)
    {
        // Use synchronous loading for WebGL to avoid async/await issues
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            return Resources.Load<Sprite>(spritePath);
        }

        ResourceRequest request = Resources.LoadAsync<Sprite>(spritePath);

        while (!request.isDone)
            await Task.Delay(1);

        return (Sprite)request.asset;
    }
}
