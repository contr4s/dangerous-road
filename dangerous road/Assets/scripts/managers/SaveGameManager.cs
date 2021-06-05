using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveFile {
    public int money;
}

public static class SaveGameManager {
    private static SaveFile _saveFile;
    private static string _filePath;

    // LOCK, if true, prevents the game from saving. This avoids issues that can happen while loading files.
    public static bool LOCK {
        get;
        private set;
    }


    static SaveGameManager() {
        LOCK = false;
        _filePath = Application.persistentDataPath + "/PA.save";

        _saveFile = new SaveFile();
    }


    public static void Save() {
        if (LOCK)
            return;

        _saveFile.money = GameManager.S.moneyManager.Money;

        string jsonSaveFile = JsonUtility.ToJson(_saveFile, true);

        File.WriteAllText(_filePath, jsonSaveFile);
    }


    public static void Load() {
        if (File.Exists(_filePath)) {
            string dataAsJson = File.ReadAllText(_filePath);

            try {
                _saveFile = JsonUtility.FromJson<SaveFile>(dataAsJson);
            }
            catch {
                Debug.LogWarning("SaveFile was incorrect.\n" + dataAsJson);
                return;
            }

            LOCK = true;

            Initialize(_saveFile);

            LOCK = false;
        }
        else {
            Debug.LogWarning("Unable to find save file. This is fine if you've never call save method ");

            LOCK = true;

            Initialize(_saveFile);

            LOCK = false;
        }
    }


    public static void DeleteSave() {
        if (File.Exists(_filePath)) {
            File.Delete(_filePath);
            _saveFile = new SaveFile();
        }
        else {
            Debug.LogWarning("Unable to find and delete save file. This is fine if you've never call save method ");
        }

        Initialize(_saveFile);
    }

    private static void Initialize(SaveFile saveFile) {
        GameManager.S.moneyManager.Money = saveFile.money;
    }
}
