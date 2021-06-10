using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class GameManager: Singleton<GameManager>
{
    public MoneyManager moneyManager;

    [SerializeField] private Car _defaultCar;
    public static Car DefaultCar { get => S._defaultCar; set => S._defaultCar = value; }

    private void Start()
    {
        SaveGameManager.Load();
        moneyManager.Initialize();
    }

    public void DeleteSave()
    {
        SaveGameManager.DeleteSave();
    }
}
