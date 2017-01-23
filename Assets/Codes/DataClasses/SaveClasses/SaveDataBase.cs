using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SaveDataBase : Singleton<SaveDataBase>
{
    private string m_SavesPath = Application.persistentDataPath + "/Saves/";
    private Dictionary<string, SaveData> m_SaveDictionary = new Dictionary<string, SaveData>();

    public SaveDataBase()
    {
    }

    public Dictionary<string, SaveData> GetSaves()
    {
        return m_SaveDictionary;
    }

    public bool HasSave(string p_SaveName)
    {
        return m_SaveDictionary.ContainsKey(p_SaveName);
    }

    public void Parse()
    {
        Clear();

        string[] l_SavesPath = Directory.GetDirectories(m_SavesPath);

        for (int i = 0; i < l_SavesPath.Length; i++)
        {
            ParseSave(l_SavesPath[i]);
        }
    }

    public void Clear()
    {
        m_SaveDictionary.Clear();
    }

    public void DeleteSave(string p_Id)
    {
        Directory.Delete(Application.persistentDataPath + "/Saves/" + p_Id, true);
    }

    private void ParseSave(string p_SavePath)
    {
        string l_PlayerDataString = string.Empty;
        string l_LocationString = string.Empty;
        string l_WorldStateString = string.Empty;
        string l_ItemsString = string.Empty;
        string l_SlotString = string.Empty;
        
        l_PlayerDataString = File.ReadAllText(p_SavePath + "/PlayerData.json");
        l_LocationString = File.ReadAllText(p_SavePath + "/Location.json");
        l_WorldStateString = File.ReadAllText(p_SavePath + "/WorldState.json");
        l_ItemsString = File.ReadAllText(p_SavePath + "/InventoryItems.json");
        l_SlotString = File.ReadAllText(p_SavePath + "/InventorySlotData.json");

        JSONObject l_PlayerDataJson = new JSONObject(l_PlayerDataString);
        JSONObject l_LocationJson = new JSONObject(l_LocationString);
        JSONObject l_WorldStateJson = new JSONObject(l_WorldStateString);
        JSONObject l_ItemsJson = new JSONObject(l_ItemsString);
        JSONObject l_SlotJson = new JSONObject(l_SlotString);

        SaveData l_SaveDate = new SaveData(l_PlayerDataJson, l_LocationJson, l_WorldStateJson, l_ItemsJson, l_SlotJson);

        m_SaveDictionary.Add(l_SaveDate.userName, l_SaveDate);
    }
}
