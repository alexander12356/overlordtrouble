using System;

public class SaveData
{
    private string m_Username;
    private string m_EnchancmenentId;
    private int m_Level;
    private DateTime m_SaveDate;
    private TimeSpan m_GameDuration;
    private JSONObject m_PlayerData;
    private JSONObject m_WorldState;
    private JSONObject m_Location;
    private JSONObject m_ItemsJson;
    private JSONObject m_SlotJson;

    public string userName
    {
        get { return m_Username; }
    }
    public int level
    {
        get { return m_Level; }
    }
    public string enchancement
    {
        get { return m_EnchancmenentId; }
    }
    public JSONObject playerData
    {
        get { return m_PlayerData; }
    }
    public JSONObject worldState
    {
        get { return m_WorldState; }
    }
    public JSONObject location
    {
        get { return m_Location; }
    }
    public JSONObject inventoryItems
    {
        get { return m_ItemsJson; }
    }
    public JSONObject equipmentSlotData
    {
        get { return m_SlotJson; }
    }
    public DateTime saveDate
    {
        get { return m_SaveDate; }
    }
    public TimeSpan gameDuration
    {
        get { return m_GameDuration; }
    }

    public SaveData(JSONObject p_PlayerData, JSONObject p_LocationData, JSONObject p_WorldStateData, JSONObject p_ItemsJson, JSONObject p_SlotJson)
    {
        Init(p_PlayerData, p_LocationData, p_WorldStateData, p_ItemsJson, p_SlotJson);
    }
        
    public void Init(JSONObject p_PlayerData, JSONObject p_LocationData, JSONObject p_WorldStateData, JSONObject p_ItemsJson, JSONObject p_SlotJson)
    {
        m_PlayerData = p_PlayerData;
        m_Username = m_PlayerData["Name"].str;
        m_Level = (int)m_PlayerData["Level"].i + 1;
        m_EnchancmenentId = m_PlayerData["Enchancement"].str;
        m_WorldState = p_WorldStateData;
        m_Location = p_LocationData;
        m_ItemsJson = p_ItemsJson;
        m_SlotJson = p_SlotJson;

        m_SaveDate = DateTime.Parse(m_WorldState["DateTime"].str);
        m_GameDuration = TimeSpan.Parse(m_WorldState["DurationTime"].str);
    }
}
