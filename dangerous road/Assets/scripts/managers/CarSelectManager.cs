using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct SelectableCar
{
    public Car prefab;
    public MeshRenderer view;
}

public class CarSelectManager: MonoBehaviour
{
    public const int MaxCarsAmount = 128;

    private static string _currentCarID;
    public static string CurrentCarID
    {
        get => _currentCarID;
        set
        {
            _currentCarID = value;            
            SaveGameManager.Save();
        }
    }

    private static List<CarParamsSO> _carData = new List<CarParamsSO>();
    public static List<CarParamsSO> CarData
    {
        get => _carData;
        set => _carData = value;
    }
    
    public static void ChangeData(CarParamsSO carParams)
    {
        for (int i = 0; i < CarData.Count; i++)
        {
            if (carParams.name == _carData[i].name)
            {
                _carData[i] = carParams;
                SaveGameManager.Save();
                return;
            }
        }
        Debug.LogWarningFormat("there is no {0} in car data", carParams);
    }

    [SerializeField] private AllCarsSO _allCars;

    [SerializeField] private CarStatUI[] _stats;
    [SerializeField] private CarUnlockWindow _unlockWindow;

    [SerializeField] private Button _rightButton;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _selectButton;
    [SerializeField] private Button _unlockButton;
    [SerializeField] private TextMeshProUGUI _selectButtonText;

    [Header("Debug values")]
    [SerializeField] List<CarParamsSO> carData;

    private readonly List<Car> _cars = new List<Car>();
    private int _curShowingCarIndex;

    private void Start()
    {
        SpawnCars();
        if (CurrentCarID is null)
        {
            CurrentCarID = _allCars.allCars[0].parametrs.name;
            ShowCar(0);
        }
        else
           ShowCar(_allCars.FindCar(CurrentCarID));

        UpdateCarData();
    }  

    private void OnEnable()
    {
        _unlockWindow.CarUnlocked += UnlockCurCar;
    }

    private void OnDisable()
    {
        _unlockWindow.CarUnlocked -= UnlockCurCar;
    }

    private void SpawnCars()
    {
        foreach (var car in _allCars.allCars)
        {
            var view = Instantiate(car, transform);
            view.transform.position = car.startPos;
            view.enabled = false;
            view.gameObject.SetActive(false);
            _cars.Add(view);
        }
    }

    public void ShowNextCar()
    {
        if (_curShowingCarIndex >= _cars.Count - 1)
            return;

        ShowCar(_curShowingCarIndex + 1);
    }

    public void ShowPrevCar()
    {
        if (_curShowingCarIndex <= 0)
            return;

        ShowCar(_curShowingCarIndex - 1);
    }

    public void SelectCurShowingCar()
    {
        CurrentCarID = _cars[_curShowingCarIndex].parametrs.name;
        UpdateSelectButton();
    }

    private void ShowCar(Car car)
    {
        for (int i = 0; i < _cars.Count; i++)
        {
            if (_cars[i].parametrs.name == car.parametrs.name)
            {
                ShowCar(i);
                return;
            }
        }
        Debug.LogWarningFormat("there is no {0} in _cars", car);
    }

    private void ShowCar(int index)
    {
        if (index < 0 || index >= _cars.Count)
        {
            Debug.LogWarning("invalid car index");
            return;
        }

        for (int i = 0; i < _cars.Count; i++)
        {
            _cars[i].gameObject.SetActive(i == index);
            if (i == index)
            {
                _curShowingCarIndex = i;
                foreach (var stat in _stats)
                {
                    stat.CarParams = _cars[i].parametrs;
                }
            }
        }

        _rightButton.interactable = (_curShowingCarIndex != _cars.Count - 1);
        _leftButton.interactable = (_curShowingCarIndex != 0);
        UpdateButtonsWhichDependOnPurchaseStatus();
    }

    private void UpdateButtonsWhichDependOnPurchaseStatus()
    {
        bool isPurchased = _cars[_curShowingCarIndex].parametrs.isPurchased;
        _unlockButton.gameObject.SetActive(!isPurchased);
        _selectButton.gameObject.SetActive(isPurchased);
        foreach (var stat in _stats)
        {
            stat.upgradeButton.gameObject.SetActive(isPurchased);
        }
        if (isPurchased)
        {
            UpdateSelectButton();
        }
        else
        {
            _unlockWindow.Setup(_cars[_curShowingCarIndex].parametrs);
        }
    }

    private void UpdateSelectButton()
    {
        if (CurrentCarID == _cars[_curShowingCarIndex].parametrs.name)
        {
            _selectButton.interactable = false;
            _selectButtonText.text = "Selected";
        }
        else
        {
            _selectButton.interactable = true;
            _selectButtonText.text = "Select";
        }
    }

    private void UnlockCurCar()
    {
        _cars[_curShowingCarIndex].parametrs.isPurchased = true;
        ChangeData(_cars[_curShowingCarIndex].parametrs);
        UpdateButtonsWhichDependOnPurchaseStatus();
    }

    private void UpdateCarData()
    {
        foreach (var car in _cars)
        {
            var parameters = car.parametrs;
            if (!CarData.Contains(parameters))
            {
                parameters.ResetParams();
                CarData.Add(parameters);
            }
        }
        for (int i = 0; i < CarData.Count;)
        {
            if (CarData[i] == null)
                CarData.RemoveAt(i);
            else
                i++;
        }
        carData = CarData;
    }
}
