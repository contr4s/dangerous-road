using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SaveFile
{
    public int money;
    public Car curSelectedCar;
    public List<CarParamsSO> carData = new List<CarParamsSO>();
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
        _saveFile.curSelectedCar = GameManager.DefaultCar;
        var parameters = GameManager.DefaultCar.parametrs;
        parameters.ResetParams();
        parameters.isPurchased = true;
        _saveFile.carData.Add(parameters);
    }

    public static void Save()
    {
        if (LOCK)
            return;

        _saveFile.money = GameManager.S.moneyManager.Money;
        _saveFile.curSelectedCar = CarSelectManager.CurrentCar;
        _saveFile.carData = CarSelectManager.CarData;

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
        Initialize(_saveFile);
    }

    private static void Initialize(SaveFile saveFile)
    {
        LOCK = true;
        GameManager.S.moneyManager.Money = saveFile.money;
        CarSelectManager.CurrentCar = saveFile.curSelectedCar;
        CarSelectManager.CarData = saveFile.carData;
        LOCK = false;
    }
}
