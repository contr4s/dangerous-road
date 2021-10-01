using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/AllCars")]
public class AllCarsSO : ScriptableObject
{
    public Car[] allCars;

    public Car FindCar(string id)
    {
        return allCars.First((car) => car.parametrs.name == id);
    }
}
