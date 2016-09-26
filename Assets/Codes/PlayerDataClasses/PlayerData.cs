using UnityEngine;

using System.Collections.Generic;
using System;

public class PlayerData : Singleton<PlayerData>
{
    #region Variables
    private string m_ProfileDataPathFile = "Data/PlayerData";
    private string m_LevelupListPathFile = "Data/LevelupList";
    private string m_ClassupListPathFile = "Data/ClassupList";

    private PlayerStat m_PlayerStat = null;
    private PlayerSkills m_PlayerSkills = null;
    private PlayerEnchancement m_PlayerEnchancement = null;

    private string m_PlayerName = "";
    private int[] m_AttackValue  = new int[2];
    private int m_Level = 0;
    private int m_Class = 0;
    private int m_Experience = 0;
    private int m_ClassImprovePoints = 0;
    private int m_StatImprovePoints = 0;
    private int m_HealthPoints = 0;
    private int m_MonstylePoints = 0;
    private List<int> m_Levelup = new List<int>();
    private List<int> m_Classup = new List<int>();
    private PanelActionHandler m_LevelupNotification = null;
    private PanelActionHandler m_ClassupNotification = null;
    #endregion

    #region Interface
    public int health
    {
        get { return m_HealthPoints;  }
        set { m_HealthPoints = value; }
    }
    public int monstylePoints
    {
        get { return m_MonstylePoints;  }
        set { m_MonstylePoints = value; }
    }
    public int statImprovePoints
    {
        get { return m_StatImprovePoints; }
        set { m_StatImprovePoints = value; }
    }
    public int[] attackValue
    {
        get { return m_AttackValue; }
        set { }
    }
    public int classImprovePoints
    {
        get { return m_ClassImprovePoints; }
        set { m_ClassImprovePoints = value; }
    }

    public void ResetData()
    {
        m_PlayerStat.ResetData();
        m_PlayerSkills.ResetData();
        m_PlayerEnchancement.ResetData();
        Parse();
        ParseLevelupList();
        ParseClassupList();
    }

    public PlayerData()
    {
        m_PlayerStat = new PlayerStat();
        m_PlayerSkills = new PlayerSkills();
        m_PlayerEnchancement = new PlayerEnchancement();
    }

    public void AddExperience(int p_Experience)
    {
        m_Experience += p_Experience;
        CheckLevelup();
    }

    public void SetData(JSONObject p_JsonObject)
    {
        Parse();
    }

    public int GetLevel()
    {
        return m_Level;
    }

    public string GetPlayerName()
    {
        return m_PlayerName;
    }

    public void AddSkills(List<SkillData> p_SkillList)
    {
        m_PlayerSkills.AddSkills(p_SkillList);
    }

    public List<SkillData> GetSkills()
    {
        return m_PlayerSkills.GetSkills();
    }

    public void SelectSkill(string p_Id)
    {
        m_PlayerSkills.SelectSkill(p_Id);
    }

    public void UnselectSkill(string p_Id)
    {
        m_PlayerSkills.UnselectSkill(p_Id);
    }

    public Dictionary<string, SkillData> GetSelectedSkills()
    {
        return m_PlayerSkills.GetSelectedSkills();
    }

    public Dictionary<string, int> GetStats()
    {
        return m_PlayerStat.GetStats();
    }

    public int GetStatValue(string p_StatId)
    {
        return m_PlayerStat.GetStatValue(p_StatId);
    }

    public void AddEnchancement(string p_EnchancementId)
    {
        m_PlayerEnchancement.AddEnchancement(p_EnchancementId);
    }

    public Sprite GetProfileAvatar()
    {
        return m_PlayerEnchancement.GetProfileAvatar();
    }

    public RuntimeAnimatorController GetAnimatorController()
    {
        return m_PlayerEnchancement.GetAnimatorController();
    }

    public Sprite GetBattleAvatar()
    {
        return m_PlayerEnchancement.GetBattleAvatar();
    }

    public int GetNextLevelupExperience()
    {
        if (m_Level < m_Levelup.Count)
        {
            return m_Levelup[m_Level] - m_Experience;
        }
        else
        {
            return m_Experience;
        }
    }

    public void InitTestStats()
    {
        ResetData();
        AddExperience(999);
    }

    public void AddLevelupNotification(PanelActionHandler p_Action)
    {
        m_LevelupNotification += p_Action;
    }

    public void AddClassupNotification(PanelActionHandler p_Action)
    {
        m_ClassupNotification += p_Action;
    }
    #endregion

    #region Private
    private void Parse()
    {
        string l_DecodedString = "";
        try
        {
            TextAsset l_TextAsset = (TextAsset)Resources.Load(m_ProfileDataPathFile);
            l_DecodedString = l_TextAsset.ToString();
        }
        catch
        {
            Debug.LogError("CANNOT READ FOR " + GetType());
        }

        JSONObject l_JSONObject = new JSONObject(l_DecodedString);

        m_PlayerName = l_JSONObject["Name"].str;

        m_AttackValue[0] = (int)l_JSONObject["AttackValue"][0].i;
        m_AttackValue[1] = (int)l_JSONObject["AttackValue"][1].i;

        m_Level = (int)l_JSONObject["Level"].f;
        m_Class = (int)l_JSONObject["Class"].f;
        m_Experience = (int)l_JSONObject["Experience"].f;
        m_PlayerStat.SetStatData(l_JSONObject["Stats"]);

        m_HealthPoints = m_PlayerStat.GetStatValue("HealthPoints");
        m_MonstylePoints = m_PlayerStat.GetStatValue("HealthPoints");
    }

    private void ParseLevelupList()
    {
        string l_DecodedString = "";
        try
        {
            TextAsset l_TextAsset = (TextAsset)Resources.Load(m_LevelupListPathFile);
            l_DecodedString = l_TextAsset.ToString();
        }
        catch
        {
            Debug.LogError("CANNOT READ FOR " + GetType());
        }

        JSONObject l_JSONObject = new JSONObject(l_DecodedString);
        
        for (int i = 0; i < l_JSONObject["Levelup"].Count; i++)
        {
            m_Levelup.Add((int)l_JSONObject["Levelup"][i].i);
        }
    }

    private void ParseClassupList()
    {
        string l_DecodedString = "";
        try
        {
            TextAsset l_TextAsset = (TextAsset)Resources.Load(m_ClassupListPathFile);
            l_DecodedString = l_TextAsset.ToString();
        }
        catch
        {
            Debug.LogError("CANNOT READ FOR " + GetType());
        }

        JSONObject l_JSONObject = new JSONObject(l_DecodedString);

        for (int i = 0; i < l_JSONObject["Classup"].Count; i++)
        {
            m_Classup.Add((int)l_JSONObject["Classup"][i].i);
        }
    }

    private void CheckLevelup()
    {
        if (m_Level < m_Levelup.Count && m_Levelup[m_Level] <= m_Experience)
        {
            m_Level++;
            m_StatImprovePoints += 4;
            LevelupNotification();

            CheckClassup();
            CheckLevelup();
        }
    }

    private void CheckClassup()
    {
        if (m_Class < m_Classup.Count && m_Classup[m_Class] <= m_Level)
        {
            m_Class++;
            m_ClassImprovePoints++;
            ClassupNotification();
        }
    }

    private void LevelupNotification()
    {
        if (m_LevelupNotification != null)
        {
            m_LevelupNotification();
            m_LevelupNotification = null;
        }
    }

    private void ClassupNotification()
    {
        if (m_ClassupNotification != null)
        {
            m_ClassupNotification();
            m_ClassupNotification = null;
        }
    }
    #endregion
}
