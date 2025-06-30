using System.IO;
using System.Collections.Generic;
using UnityEngine;


public static class SaveManager
{
    private static readonly string savePath = Application.persistentDataPath + "/save.json";
    public static bool HasLoadedSave = false;
    public static void Save(List<DieSettings> dice, int bodies, int brains, HashSet<string> unlockedDoors,
        HashSet<string> unlockedDice, float mainBaseTBw, float mainBaseTBh, float door1TBw, float door1TBh,
        float door2TBw, float door2TBh, float door3TBw, float door3TBh, float sicknessTBw, float sicknessTBh)
    {
        SaveData data = new SaveData
        {
            diePool = dice,
            brains = brains,
            bodies = bodies,
            unlockedDoors = new List<string>(unlockedDoors),
            unlockedDice = new List<string>(unlockedDice),
            mainBaseTBw = mainBaseTBw,
            mainBaseTBh = mainBaseTBh,
            door1TBw = door1TBw,
            door1TBh = door1TBh,
            door2TBw = door2TBw,
            door2TBh = door2TBh,
            door3TBw = door3TBw,
            door3TBh = door3TBh,
            sicknessTBw = sicknessTBw,
            sicknessTBh = sicknessTBh
        };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    public static SaveData Load()
    {
        if (!File.Exists(savePath))
            return null;

        HasLoadedSave = true;
        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<SaveData>(json);
    }
    public static void ClearSaveFile()
    {
        string path = Application.persistentDataPath + "/save.json";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted");
        }
    }
}
