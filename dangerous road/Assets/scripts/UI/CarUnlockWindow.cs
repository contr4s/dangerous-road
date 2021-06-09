using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarUnlockWindow : MonoBehaviour
{
    public const string price = "price";

    public Action CarUnlocked;

    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private Button _unlockButton;

    private CarParamsSO _curCarParams;

    public void Setup(CarParamsSO carParams)
    {
        _curCarParams = carParams;
        _title.text = carParams.name;
        _price.text = string.Format("{0}: {1}$", price, carParams.purchasePrice);
        if (carParams.purchasePrice > GameManager.S.moneyManager.Money)
            _unlockButton.interactable = false;
    }

    public void UnlockButton()
    {
        if (_curCarParams is null)
            return;

        GameManager.S.moneyManager.Money -= _curCarParams.purchasePrice;
        CarUnlocked?.Invoke();
    }
}
