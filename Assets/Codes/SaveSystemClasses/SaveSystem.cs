using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine.SceneManagement;

public class SaveSystem : Singleton<SaveSystem>
{
    private string m_LocationId = string.Empty;
    private Dictionary<string, LocationSave> m_LocationSaves = new Dictionary<string, LocationSave>();
    private DateTime m_StartDate;
    private TimeSpan m_DurationTime;

    public SaveSystem()
    {
#if UNITY_EDITOR
        m_LocationId = SceneManager.GetActiveScene().name;
        m_LocationSaves.Add(m_LocationId, new LocationSave(m_LocationId));
#endif
    }

    public void Init(string p_LocationId)
    {
        m_LocationId = p_LocationId;
        if (!m_LocationSaves.ContainsKey(m_LocationId))
        {
            m_LocationSaves.Add(m_LocationId, new LocationSave(m_LocationId));
        }
    }

    public void ClearCache()
    {
        m_LocationSaves[m_LocationId].ClearCache();
    }

    public void AddActor(JourneyActor p_JourneyActor)
    {
        m_LocationSaves[m_LocationId].AddActor(p_JourneyActor);
    }

    public void AddDoor(FrontDoor p_Door)
    {
        m_LocationSaves[m_LocationId].AddDoor(p_Door);
    }

    public void AddTrigger(GameEventTrigger p_GameEventTrigger)
    {
        m_LocationSaves[m_LocationId].AddTrigger(p_GameEventTrigger);
    }

    public void AddCheckCounter(CheckCounterAction p_CheckCounter)
    {
        m_LocationSaves[m_LocationId].AddCheckCounter(p_CheckCounter);
    }

    public void AddWarp(Warp p_Warp)
    {
        m_LocationSaves[m_LocationId].AddWarp(p_Warp);
    }

    public void AddRoomGenerator(RoomEnemyGenerator p_RoomGenerator)
    {
        m_LocationSaves[m_LocationId].AddRoomGenerator(p_RoomGenerator);
    }

    public void AddAnimationObject(AnimationObject p_AnimationObject)
    {
        m_LocationSaves[m_LocationId].AddAnimationObject(p_AnimationObject);
    }

    public void ActorDie(string p_Id)
    {
        m_LocationSaves[m_LocationId].ActorDie(p_Id);
    }

    public void GameEventDestroy(string p_Id)
    {
        m_LocationSaves[m_LocationId].GameEventDestroy(p_Id);
    }

    public void SetCheckCounterValue(string p_Id, int p_Value)
    {
        m_LocationSaves[m_LocationId].SetCheckCounterValue(p_Id, p_Value);
    }

    public void SaveToMemory()
    {
        m_LocationSaves[m_LocationId].Save();
    }

    public void LoadFromMemory()
    {
        m_LocationSaves[m_LocationId].Load();
    }

    public void SaveToFile(string p_SenderLocation)
    {
        if (!Directory.Exists(PlayerData.GetInstance().GetSavePath()))
        {
            Directory.CreateDirectory(PlayerData.GetInstance().GetSavePath());
        }

        PlayerData.GetInstance().SaveToDisk();
        PlayerInventory.GetInstance().SaveToDisk();
        WorldStateSaveToDisk();
        LocationSaveToDisk(p_SenderLocation);

        DialogManager.GetInstance().StartDialog("SaveComplete");
        JourneySystem.GetInstance().SetControl(ControlType.Panel);
    }

    public void LoadFromFile(JSONObject p_WorldStateJson)
    {
        m_DurationTime += TimeSpan.Parse(p_WorldStateJson["DurationTime"].str);

        for (int i = 0; i < p_WorldStateJson["Locations"].Count; i++)
        {
            string l_LocationId = p_WorldStateJson["Locations"][i]["Id"].str;
            Init(l_LocationId);

            m_LocationSaves[l_LocationId].LoadFromFile(p_WorldStateJson["Locations"][i]);
        }
    }

    public void StartDuration()
    {
        m_StartDate = DateTime.Now;
    }

    private void WorldStateSaveToDisk()
    {
        JSONObject l_LocationsJson = new JSONObject();
        JSONObject l_WorldStateJson = new JSONObject();

        l_WorldStateJson.AddField("DateTime", DateTime.Now.ToString());
        m_DurationTime += (DateTime.Now - m_StartDate);
        m_DurationTime = new TimeSpan(m_DurationTime.Hours, m_DurationTime.Minutes, m_DurationTime.Seconds);
        l_WorldStateJson.AddField("DurationTime", m_DurationTime.ToString());

        foreach (LocationSave l_LocationSave in m_LocationSaves.Values)
        {
            l_LocationsJson.Add(l_LocationSave.GetJson());
        }

        
        l_WorldStateJson.AddField("Locations", l_LocationsJson);

        File.WriteAllText(PlayerData.GetInstance().GetSavePath() + "WorldState.json", l_WorldStateJson.Print(true));
    }

    private void LocationSaveToDisk(string p_SenderLocation)
    {
        JSONObject l_LocationJson = new JSONObject();
        l_LocationJson.AddField("Scene", SceneManager.GetActiveScene().name);
        l_LocationJson.AddField("SenderLocation", p_SenderLocation);
        l_LocationJson.AddField("TargetRoom", RoomSystem.GetInstance().currentRoomId);

        File.WriteAllText(PlayerData.GetInstance().GetSavePath() + "Location.json", l_LocationJson.Print(true));
    }
}