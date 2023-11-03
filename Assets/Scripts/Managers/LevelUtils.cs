using ScriptsPhysicsAndMechanics;
using UnityEngine;
public static class LevelUtils
{
    public static GameObject InstantiateCar(CarData carData)
    {
        var car = ResourceHelper.InstantiatePrefab(carData.prefabPath, null);
        var carController = car.GetComponent<VehicleController>();
        carController.Init(carData);
        return car;
    }

    public static GameObject InstantiateStage(StageData stageData)
    {
        var stage = ResourceHelper.InstantiatePrefab(stageData.prefabPath, null);
        stage.transform.position = Vector3.zero;
        stage.transform.localScale = Vector3.one;
        return stage;
    }
}
