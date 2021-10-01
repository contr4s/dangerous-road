using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SaveFile
{
    public int money;
    public string curSelectedCarID;
    public CarParamsSO[] carData = new CarParamsSO[CarSelectManager.MaxCarsAmount];
    public bool isSoundMuted = false;
    public bool isMusicMuted = false;
    public eTurnInputType turnInputType = eTurnInputType.touch;
}

public static class SaveGameManager
{
    private static SaveFile _saveFile;
    private static readonly string _filePath;

    // LOCK, if true, prevents the game from saving. This avoids issues that can happen while loading files.
    public static bool LOCK
    {
        get;
        private set;
    }


    static SaveGameManager()
    {
        LOCK = false;
        _filePath = Application.persistentDataPath + "/PA.save";

        InitSaveFile();
    }

    private static void InitSaveFile()
    {
        _saveFile = new SaveFile();
        _saveFile.curSelectedCarID = GameManager.DefaultCar.parametrs.name;
        var parameters = GameManager.DefaultCar.parametrs;
        parameters.ResetParams();
        parameters.isPurchased = true;
        _saveFile.carData[0] = parameters;
    }

    public static void Save()
    {
        if (LOCK)
            return;

        _saveFile.money = GameManager.S.moneyManager.Money;
        _saveFile.curSelectedCarID = CarSelectManager.CurrentCarID;
        _saveFile.carData = CarSelectManager.CarData.ToArray();
        _saveFile.isSoundMuted = GameplaySoundManager.muted;
        _saveFile.isMusicMuted = MusisManager.muted;
        _saveFile.turnInputType = InputManager.turnInputType;

        string jsonSaveFile = JsonUtility.ToJson(_saveFile, true);

        File.WriteAllText(_filePath, jsonSaveFile);
    }


    public static void Load()
    {
        if (File.Exists(_filePath))
        {
            string dataAsJson = File.ReadAllText(_filePath);

            try
            {
                _saveFile = JsonUtility.FromJson<SaveFile>(dataAsJson);
            }
            catch
            {
                Debug.LogWarning("SaveFile was incorrect.\n" + dataAsJson);
                return;
            }

            Initialize(_saveFile);
        }
        else
        {
            Debug.LogWarning("Unable to find save file. This is fine if you've never call save method ");
            InitSaveFile();
            Initialize(_saveFile);
        }
    }


    public static void DeleteSave()
    {
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }
        else
        {
            Debug.LogWarning("Unable to find and delete save file. This is fine if you've never call save method ");
        }
        InitSaveFile();
        Initialize(_saveFile);
    }

    private static void Initialize(SaveFile saveFile)
    {
        LOCK = true;
        GameManager.S.moneyManager.Money = saveFile.money;
        CarSelectManager.CurrentCarID = saveFile.curSelectedCarID;
        CarSelectManager.CarData = saveFile.carData.ToList();
        GameplaySoundManager.muted = saveFile.isSoundMuted;
        MusisManager.muted = saveFile.isMusicMuted;
        InputManager.turnInputType = saveFile.turnInputType;
        LOCK = false;
    }
}
