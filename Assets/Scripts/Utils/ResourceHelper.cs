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
        ResourceRequest request = Resources.LoadAsync(prefabPath, typeof(GameObject));

        while (!request.isDone)
            await Task.Yield(); // Yield to allow Unity to load the resource asynchronously

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
        ResourceRequest request = Resources.LoadAsync<Sprite>(spritePath);
        await Task.Yield(); 
        while (!request.isDone)
            await Task.Yield(); 

        return (Sprite)request.asset;
    }
}
