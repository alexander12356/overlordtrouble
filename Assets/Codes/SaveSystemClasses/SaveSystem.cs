using System.Collections.Generic;

using UnityEngine.SceneManagement;

public class SaveSystem : Singleton<SaveSystem>
{
    private string m_LocationId = string.Empty;
    private Dictionary<string, LocationSave> m_LocationSaves = new Dictionary<string, LocationSave>();

    public SaveSystem()
    {
#if UNITY_EDITOR
        m_LocationId = SceneManager.GetActiveScene().name;
        m_LocationSaves.Add(m_LocationId, new LocationSave());
#endif
    }

    public void Init(string p_LocationId)
    {
        m_LocationId = p_LocationId;
        if (!m_LocationSaves.ContainsKey(m_LocationId))
        {
            m_LocationSaves.Add(m_LocationId, new LocationSave());
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

    public void ActorDie(string p_Id)
    {
        m_LocationSaves[m_LocationId].ActorDie(p_Id);
    }

    public void GameEventDestroy(string p_Id)
    {
        m_LocationSaves[m_LocationId].GameEventDestroy(p_Id);
    }

    public void SaveToMemory()
    {
        m_LocationSaves[m_LocationId].Save();
    }

    public void LoadFromMemory()
    {
        m_LocationSaves[m_LocationId].Load();
    }

    public void SaveToFile()
    {
    }

    public void LoadFromFile()
    {
    }
}