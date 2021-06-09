using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private CarStatUI[] _stats;
    [SerializeField] private CarUnlockWindow _unlockWindow;

    [SerializeField] private Button _rightButton;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _selectButton;
    [SerializeField] private Button _unlockButton;
    [SerializeField] private TextMeshProUGUI _selectButtonText;

    private int _curShowingCarIndex;

    private void Start()
    {
        if (CurrentCar is null)
        {
            CurrentCar = _cars[0].prefab;
        }
        else
            ShowCar(CurrentCar);
    }

    private void OnEnable()
    {
        _unlockWindow.CarUnlocked += UnlockCurCar;
    }

    private void OnDisable()
    {
        _unlockWindow.CarUnlocked -= UnlockCurCar;
    }

    public void ShowNextCar()
    {
        if (_curShowingCarIndex >= _cars.Length - 1)
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
        CurrentCar = _cars[_curShowingCarIndex].prefab;
        UpdateSelectButton();
    }

    private void ShowCar(Car car)
    {
        for (int i = 0; i < _cars.Length; i++)
        {
            if (_cars[i].prefab == car)
            {
                ShowCar(i);
                return;
            }
        }
        Debug.LogWarningFormat("there is no {0} in _cars", car);
    }

    private void ShowCar(int index)
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
                _curShowingCarIndex = i;
                foreach (var stat in _stats)
                {
                    stat.CarParams = _cars[i].prefab.parametrs;
                }
            }
        }

        _rightButton.interactable = (_curShowingCarIndex != _cars.Length - 1);
        _leftButton.interactable = (_curShowingCarIndex != 0);
        UpdateButtonsWhichDependOnPurchaseStatus();
    }

    private void UpdateButtonsWhichDependOnPurchaseStatus()
    {
        bool isPurchased = _cars[_curShowingCarIndex].prefab.parametrs.isPurchased;
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
            _unlockWindow.Setup(_cars[_curShowingCarIndex].prefab.parametrs);
        }
    }

    private void UpdateSelectButton()
    {
        if (CurrentCar == _cars[_curShowingCarIndex].prefab)
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
        _cars[_curShowingCarIndex].prefab.parametrs.isPurchased = true;
        UpdateButtonsWhichDependOnPurchaseStatus();
    }
}
