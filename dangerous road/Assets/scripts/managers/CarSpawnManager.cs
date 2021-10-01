using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.VFX;

public class CarSpawnManager: MonoBehaviour
{
    public static Action<Car> carSpawned;
    public static bool canTurn = false;

    [SerializeField] private AllCarsSO _allCars;
    [SerializeField] private Car _defaultCar;
    [SerializeField] private UIManager _uIManager;
    [SerializeField] private VisualEffect _wind;

    [SerializeField] PlayableDirector _playableDirector;
    [SerializeField] GameplaySoundManager _soundManager;

    private static Car _spawnedCar;   

    private void Start()
    {
        print(CarSelectManager.CurrentCarID);
        if (string.IsNullOrEmpty(CarSelectManager.CurrentCarID))
        {
            SpawnCar(_defaultCar);
        }
        else
        {
            SpawnCar(_allCars.FindCar(CarSelectManager.CurrentCarID));
        }
        canTurn = false;
        _uIManager.SetActiveAllHudElements(false);
        StartCoroutine(WaitUntillClipPlayed());
    }

    public static bool TryTurn(bool turningRight)
    {
        if (canTurn)
            _spawnedCar.TurnButton(turningRight);
        return canTurn;
    }

    private void SpawnCar(Car car)
    {
        _spawnedCar = Instantiate(car, car.startPos, Quaternion.identity);
        _spawnedCar.uIManager = _uIManager;
        _spawnedCar.wind = _wind;
        _spawnedCar.soundManager = _soundManager;
        carSpawned?.Invoke(_spawnedCar);
    }

    private IEnumerator WaitUntillClipPlayed()
    {
        yield return new WaitForSeconds((float)_playableDirector.duration);
        yield return StartCoroutine(_soundManager.PlaySoundCoroutine(eSoundType.startEngine));
        _uIManager.SetActiveAllHudElements(true);
        canTurn = true;
        StartCoroutine(_spawnedCar.Acelerate());
        _soundManager.PlaySound(eSoundType.engine);
        _soundManager.PlaySound(eSoundType.drive);
    }
}
