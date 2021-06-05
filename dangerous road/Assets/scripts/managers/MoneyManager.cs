using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

[Serializable]
public class MoneyManager
{
    [SerializeField] private TextMeshProUGUI _moneyDisplay;

    [SerializeField] private int _money = 0;
    public int Money
    {
        get => _money;
        set
        {
            if (value < 0)
                Debug.LogError("collected money can't be less than 0");

            _money = value;
            UpdateMoneyDisplay();
            SaveGameManager.Save();          
        }
    }

    public void Initialize()
    {
        UpdateMoneyDisplay();
    }

    private void UpdateMoneyDisplay()
    {
        if (_moneyDisplay)
            _moneyDisplay.text = string.Format("Total money: {0}$", _money.ToString());
    }
}
