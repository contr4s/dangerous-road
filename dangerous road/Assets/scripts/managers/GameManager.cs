using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class GameManager: Singleton<GameManager>
{
    public MoneyManager moneyManager;

    private void Start()
    {
        SaveGameManager.Load();
        moneyManager.Initialize();
    }
}
