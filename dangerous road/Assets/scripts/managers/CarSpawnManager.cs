using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawnManager : MonoBehaviour
{
    public static Action<Car> carSpawned;

    [SerializeField] private Car _defaultCar;
    [SerializeField] private UIManager _uIManager;

    private Car _spawnedCar;

    private void Start()
    {
        if (CarSelectManager.CurrentCar is null)
        {
            SpawnCar(_defaultCar);
        }
        else
        {
            SpawnCar(CarSelectManager.CurrentCar);
        }
           
    }

    public void TurnButton(bool turningRight)
    {
        _spawnedCar.TurnButton(turningRight);
    }

    private void SpawnCar(Car car)
    {
        _spawnedCar = Instantiate(car, car.startPos, Quaternion.identity);
        _spawnedCar.uIManager = _uIManager;
        carSpawned?.Invoke(_spawnedCar);
    }
}
