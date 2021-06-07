using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public static Action<Car> carSpawned;

    [SerializeField] private Car[] _cars;
    [SerializeField] private UIManager _uIManager;

    private Car _spawnedCar;
    private int _curCarIndex;

    private void Start()
    {
        if (CarSelectManager.CurrentCar is null)
        {
            CarSelectManager.CurrentCar = _cars[0];
        }
        else
            SelectCar(CarSelectManager.CurrentCar);
    }

    public void TurnButton(bool turningRight)
    {
        _spawnedCar.TurnButton(turningRight);
    }

    private void SelectCar(int index)
    {
        if (index < 0 || index >= _cars.Length)
        {
            Debug.LogWarning("invalid car index");
            return;
        }

        for (int i = 0; i < _cars.Length; i++)
        {
            if (i == index)
            {
                _curCarIndex = i;
                _spawnedCar = Instantiate(_cars[i], _cars[i].startPos, Quaternion.identity);
                _spawnedCar.uIManager = _uIManager;
                Camera.main.transform.SetParent(_spawnedCar.transform);
                carSpawned?.Invoke(_spawnedCar);
            }
        }
    }

    private void SelectCar(Car car)
    {
        for (int i = 0; i < _cars.Length; i++)
        {
            if (_cars[i] == car)
            {
                SelectCar(i);
                return;
            }
        }
        Debug.LogWarningFormat("there is no {0} in _cars", car);
    }
}
