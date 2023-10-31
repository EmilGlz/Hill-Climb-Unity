using UnityEngine;
public static class CarCreator
{
    public static GameObject InstantiateCar(CarData carData)
    {
        var car = ResourceHelper.InstantiatePrefab(carData.prefabPath, null);
        return car;
    }
}
