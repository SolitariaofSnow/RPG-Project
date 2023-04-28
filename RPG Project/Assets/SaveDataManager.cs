using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public interface DataObject {
    public virtual string Save();
    public virtual void Load(string data);
    public string DataName { get; }
    private const string DataName;
}

public static class SaveDataManager {
    public static void Save(string filename) {
        if (saveData != null) saveData.Clear();
        foreach (DataObject d in SavableData)
            saveData.Add(d.DataName, d.Save());
        string json = JsonUtility.ToJson(saveData);
        System.IO.File.WriteAllText(filename, json);
    }
    public static void Load(string filename) {
        string json = System.IO.File.ReadAllText(filename);
        loadData = JsonUtility.FromJson<Dictionary<string, string>>(json);

        foreach(KeyValuePair<string, string> d in saveData) {
            DataObject? obj = null;
            // locate the correct DataObject by name
            foreach (DataObject o in SavableData) {
                if (o.DataName == d.Key) {
                    obj = o;
                    break;
                }
            }
            if (obj == null) throw new System.FormatException(String.Format("Load Failed: {0} not found", d.Key));
            obj.Load(d.Value);
        }
    }
    // Register DataObject to be saved
    public static void Register(ref DataObject obj) {
        System.ArgumentException e = new System.ArgumentException(
                String.Format("DataObject {0} already registered", obj.DataName));

        // Enforce no duplicate DataObjects
        if(SavableData.Contains(obj))
            throw e;
        foreach(DataObject o in SavableData)
            if(o.DataName == obj.DataName)
                throw e;
        SavableData.Add(obj);
    }
    private static IDictionary<string, string>? SaveData = new Dictionary<string, string>();
    private static IDictionary<string, string>? LoadData = new Dictionary<string, string>();
    private static List<DataObject> SavableData = new List<DataObject>();
}



