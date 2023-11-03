using ScriptsPhysicsAndMechanics;
using UnityEngine;
public static class CarCreator
{
    public static GameObject InstantiateCar(CarData carData)
    {
        var car = ResourceHelper.InstantiatePrefab(carData.prefabPath, null);
        var carController = car.GetComponent<VehicleController>();
        carController.Init(carData);
        return car;
    }
}
