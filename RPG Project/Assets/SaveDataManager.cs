using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class DataObject {
    public abstract string Save();
    public abstract void Load(string data);
    public string DataName { get; }
}

public static class SaveDataManager {
    public static void Save(string filename = Config.SaveFile) {
        if (SaveData != null) SaveData.Clear();
        foreach (DataObject d in SavableData)
            SaveData.Add(d.DataName, d.Save());
        string json = JsonUtility.ToJson(SaveData);
        // TODO: Encrypt save data
        System.IO.File.WriteAllText(filename, json);
    }
    public static void Load(string filename = Config.SaveFile) {
        string json = System.IO.File.ReadAllText(filename);
        // TODO: decrypt save data
        LoadData = JsonUtility.FromJson<Dictionary<string, string>>(json);

        foreach(KeyValuePair<string, string> d in SaveData) {
#nullable enable
            DataObject? obj = null;
            // locate the correct DataObject by name
            foreach (DataObject o in SavableData) {
                if (o.DataName == d.Key) {
                    obj = o;
                    break;
                }
            }
            if (obj == null) throw new System.FormatException(string.Format("Load Failed: {0} not found", d.Key));
            obj.Load(d.Value);
#nullable disable
        }
    }
    // Register DataObject to be saved
    public static void Register(ref DataObject obj) {
        System.ArgumentException e = new System.ArgumentException(
                string.Format("DataObject {0} already registered", obj.DataName));

        // Enforce no duplicate DataObjects
        if(SavableData.Contains(obj))
            throw e;
        foreach(DataObject o in SavableData)
            if(o.DataName == obj.DataName)
                throw e;
        SavableData.Add(obj);
    }
    public static bool CanLoad(string filename = Config.SaveFile) {
        return System.IO.File.Exists(filename);
    }
    private static IDictionary<string, string> SaveData = new Dictionary<string, string>();
    private static IDictionary<string, string> LoadData = new Dictionary<string, string>();
    private static List<DataObject> SavableData = new List<DataObject>();
}



