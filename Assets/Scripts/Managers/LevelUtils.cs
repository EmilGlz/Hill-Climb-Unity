using Scripts.Managers;
using UnityEngine;
public static class LevelUtils
{
    public static GameObject InstantiateCar(CarData carData)
    {
        var car = ResourceHelper.InstantiatePrefab(carData.prefabPath, null);
        car.transform.position = Vector3.zero + Vector3.up * 5f + Vector3.right * 5f;
        return car;
    }

    public static GameObject InstantiateStage(StageData stageData)
    {
        var stage = ResourceHelper.InstantiatePrefab(stageData.prefabPath, null);
        stage.transform.position = Vector3.zero;
        stage.transform.localScale = Vector3.one;
        return stage;
    }

    public static void UpdateSkyBackground(StageData data)
    {
        var backgroundSprite = Utils.FindGameObject("Background", GameManager.Instance.MainCamera.gameObject).GetComponent<SpriteRenderer>();
        backgroundSprite.gameObject.SetActive(true);
        backgroundSprite.sprite = data.skySprite;
        FitCameraToSprite();



        void FitCameraToSprite()
        {
            if (backgroundSprite != null)
            {
                float spriteHeight = backgroundSprite.bounds.size.y;
                float orthographicSize = spriteHeight / 2.0f;
                GameManager.Instance.MainCamera.orthographicSize = orthographicSize;
            }
        }
    }

    public static float GetWidth(GameObject ground)
    {
        var startPos = Utils.FindGameObject("GroundStart", ground).transform.position;
        var endPos = Utils.FindGameObject("GroundEnd", ground).transform.position;
        return endPos.x - startPos.x;
    }

}
