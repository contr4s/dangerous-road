using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.VFX;

public class CarSpawnManager: MonoBehaviour
{
    public static Action<Car> carSpawned;

    [SerializeField] private Car _defaultCar;
    [SerializeField] private UIManager _uIManager;
    [SerializeField] private VisualEffect _wind;

    [SerializeField] PlayableDirector _playableDirector;

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
        StartCoroutine(WaitUntillClipPlayed());   
    }

    public void TurnButton(bool turningRight)
    {
        _spawnedCar.TurnButton(turningRight);
    }

    private void SpawnCar(Car car)
    {
        _spawnedCar = Instantiate(car, car.startPos, Quaternion.identity);
        _spawnedCar.uIManager = _uIManager;
        _spawnedCar.wind = _wind;
        carSpawned?.Invoke(_spawnedCar);
    }

    private IEnumerator WaitUntillClipPlayed()
    {
        yield return new WaitForSeconds((float)_playableDirector.duration);
        _spawnedCar.canAccelerate = true;
    }
}
