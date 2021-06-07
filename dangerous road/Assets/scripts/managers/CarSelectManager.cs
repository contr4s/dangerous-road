using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SelectableCar
{
    public Car prefab;
    public MeshRenderer view;
}

public class CarSelectManager: MonoBehaviour
{
    private static Car _currentCar;
    public static Car CurrentCar
    {
        get => _currentCar;
        set
        {
            _currentCar = value;
            SaveGameManager.Save();
        }
    }

    [SerializeField] private SelectableCar[] _cars;

    private int _curCarIndex;

    private void Start()
    {
        if (CurrentCar is null)
        {
            CurrentCar = _cars[0].prefab;
        }
        else
            SelectCar(CurrentCar);
    }

    public void SelectNextCar()
    {
        if (_curCarIndex >= _cars.Length - 1)
            return;

        SelectCar(_curCarIndex + 1);
    }

    public void SelectPrevCar()
    {
        if (_curCarIndex <= 0)
            return;

        SelectCar(_curCarIndex - 1);
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
            _cars[i].view.gameObject.SetActive(i == index);
            if (i == index)
            {
                CurrentCar = _cars[i].prefab;
                _curCarIndex = i;
            }
        }
    }

    private void SelectCar(Car car)
    {
        for (int i = 0; i < _cars.Length; i++)
        {
            if (_cars[i].prefab == car)
            {
                SelectCar(i);
                return;
            }
        }
        Debug.LogWarningFormat("there is no {0} in _cars", car);
    }
}
