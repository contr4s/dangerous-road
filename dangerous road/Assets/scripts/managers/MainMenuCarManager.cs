using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCarManager : MonoBehaviour
{
    [SerializeField] AllCarsSO _allCars;

    private readonly List<Car> _spawnedCars = new List<Car>();

    private void Start()
    {
        SpawnCar();
    }

    private void SpawnCars()
    {
        foreach(var car in _allCars.allCars)
        {
            var view = Instantiate(car, transform);
            view.transform.position = car.startPos;
            view.enabled = false;
            view.gameObject.SetActive(false);
            _spawnedCars.Add(view);
        }
    }

    private void SpawnCar()
    {
        var carToSpawn = _allCars.FindCar(CarSelectManager.CurrentCarID) ?? _allCars.allCars[0];
        var car = Instantiate(carToSpawn, transform);
        car.transform.position = car.startPos;
        car.enabled = false;
    }
}
