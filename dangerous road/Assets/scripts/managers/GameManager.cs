using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class GameManager: Singleton<GameManager>
{
    [SerializeField] private TextMeshProUGUI _moneyDisplay;
    NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

    [SerializeField] private int _money = 0;
    public int Money
    {
        get => _money;
        set
        {
            if (value < 0)
                Debug.LogError("collected money can't be less than 0");
            _money = value;
        }
    }

    private void Start()
    {
        SaveGameManager.Load();
        if (_moneyDisplay)
            _moneyDisplay.text = string.Format("Total money: {0}", _money.ToString("C", nfi));
    }
}
