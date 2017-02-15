using System.IO;
using System.Collections.Generic;

using UnityEngine;
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
    public int specialPoints
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

    public PlayerData()
    {
        m_PlayerStat = new PlayerStat();
        m_PlayerSkills = new PlayerSkills();
        m_PlayerEnchancement = new PlayerEnchancement();

        ParseLevelupList();
        ParseClassupList();
    }

    public void ClearData()
    {
        Clear();
        m_PlayerStat.Clear();
        m_PlayerSkills.Clear();
        m_PlayerSkills.ClearSelectedSkills();
        m_PlayerEnchancement.Clear();
    }

    public void NewGameDataInit()
    {
        ClearData();
        Parse();

        AddEnchancement("DewElemental");
        m_PlayerSkills.DefaultSkillSelection();
    }

    public void LoadData(JSONObject p_PlayerData)
    {
        ClearData();

        m_PlayerName = p_PlayerData["Name"].str;

        m_AttackValue[0] = (int)p_PlayerData["AttackValue"][0].i;
        m_AttackValue[1] = (int)p_PlayerData["AttackValue"][1].i;

        m_Level = (int)p_PlayerData["Level"].i;
        m_Class = (int)p_PlayerData["Class"].i;
        m_Experience = (int)p_PlayerData["Experience"].i;
        m_StatImprovePoints = (int)p_PlayerData["StatImprovePoints"].i;
        m_ClassImprovePoints = (int)p_PlayerData["ClassImprovePoints"].i;

        m_PlayerStat.SetStatData(p_PlayerData["Stats"]);

        m_HealthPoints = m_PlayerStat.GetStatValue("HealthPoints");
        m_MonstylePoints = m_PlayerStat.GetStatValue("HealthPoints");

        LoadSpecials(p_PlayerData["Specials"]);
        LoadSelectedSpecials(p_PlayerData["SelectedSpecials"]);

        m_PlayerEnchancement.SetEnchancementId(p_PlayerData["Enchancement"].str);
    }

    public void SaveToDisk()
    {
        JSONObject l_PlayerDataJson = new JSONObject();

        l_PlayerDataJson.AddField("Name", m_PlayerName);

        JSONObject l_AttackValueJson = new JSONObject();
        l_AttackValueJson.Add(m_AttackValue[0]);
        l_AttackValueJson.Add(m_AttackValue[1]);
        l_PlayerDataJson.AddField("AttackValue", l_AttackValueJson);

        l_PlayerDataJson.AddField("Level", m_Level);
        l_PlayerDataJson.AddField("Class", m_Class);
        l_PlayerDataJson.AddField("Experience", m_Experience);
        l_PlayerDataJson.AddField("StatImprovePoints", m_StatImprovePoints);
        l_PlayerDataJson.AddField("ClassImprovePoints", m_ClassImprovePoints);

        JSONObject l_StatJson = m_PlayerStat.GetJson();
        l_PlayerDataJson.AddField("Stats", l_StatJson);

        l_PlayerDataJson.AddField("Enchancement", m_PlayerEnchancement.GetCurrentEnchancement());

        JSONObject l_SpecialsJson = m_PlayerSkills.GetSkillsJson();
        l_PlayerDataJson.AddField("Specials", l_SpecialsJson);

        JSONObject l_SelectedSpecialsJson = m_PlayerSkills.GetSelectedSkillsJson();
        l_PlayerDataJson.AddField("SelectedSpecials", l_SelectedSpecialsJson);

        File.WriteAllText(GetSavePath() + "PlayerData.json", l_PlayerDataJson.Print(true));
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

    public void SetPlayerName(string p_Name)
    {
        m_PlayerName = p_Name;
    }

    public void AddSkills(List<SpecialData> p_SkillList)
    {
        m_PlayerSkills.AddSkills(p_SkillList);
    }

    public void AddSelectedSkills(List<SpecialData> p_SkillList, bool overwrite = false)
    {
        m_PlayerSkills.AddSelectedSkills(p_SkillList, overwrite);
    }

    public List<SpecialData> GetSkills()
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

    public void RemoveFirstSelectedSkill()
    {
        m_PlayerSkills.RemoveFirstSelectedSkill();
    }

    public List<SpecialData> GetSelectedSkills()
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
        m_PlayerEnchancement.UpgradeClass(p_EnchancementId);
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
            return 0;
        }
    }

    public void InitTestStats()
    {
        NewGameDataInit();
    }

    public void AddLevelupNotification(PanelActionHandler p_Action)
    {
        m_LevelupNotification += p_Action;
    }

    public void AddClassupNotification(PanelActionHandler p_Action)
    {
        m_ClassupNotification += p_Action;
    }

    public string GetCurrentEnchancement()
    {
        return m_PlayerEnchancement.GetCurrentEnchancement();
    }

    public string GetSavePath()
    {
        return Application.persistentDataPath + "/Saves/" + m_PlayerName + Path.DirectorySeparatorChar;
    }
    #endregion

    #region Private
    private void Parse()
    {
        string l_DecodedString = "";

        if (File.Exists(Application.streamingAssetsPath + "/" + m_ProfileDataPathFile + ".txt"))
        {
            l_DecodedString = File.ReadAllText(Application.streamingAssetsPath + "/" + m_ProfileDataPathFile + ".txt");
        }
        else
        {
            try
            {
                TextAsset l_TextAsset = (TextAsset)Resources.Load(m_ProfileDataPathFile);
                l_DecodedString = l_TextAsset.ToString();
            }
            catch
            {
                Debug.LogError("CANNOT READ FOR " + GetType());
            }
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

        if (File.Exists(Application.streamingAssetsPath + "/" + m_LevelupListPathFile + ".json"))
        {
            l_DecodedString = File.ReadAllText(Application.streamingAssetsPath + "/" + m_LevelupListPathFile + ".json");
        }
        else
        {
            try
            {
                TextAsset l_TextAsset = (TextAsset)Resources.Load(m_LevelupListPathFile);
                l_DecodedString = l_TextAsset.ToString();
            }
            catch
            {
                Debug.LogError("CANNOT READ FOR " + GetType());
            }
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

        if (File.Exists(Application.streamingAssetsPath + "/" + m_ClassupListPathFile + ".json"))
        {
            l_DecodedString = File.ReadAllText(Application.streamingAssetsPath + "/" + m_ClassupListPathFile + ".json");
        }
        else
        {
            try
            {
                TextAsset l_TextAsset = (TextAsset)Resources.Load(m_ClassupListPathFile);
                l_DecodedString = l_TextAsset.ToString();
            }
            catch
            {
                Debug.LogError("CANNOT READ FOR " + GetType());
            }
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

    private void Clear()
    {
        m_PlayerName = "";
        m_AttackValue = new int[2];
        m_Level = 0;
        m_Class = 0;
        m_Experience = 0;
        m_ClassImprovePoints = 0;
        m_StatImprovePoints = 0;
        m_HealthPoints = 0;
        m_MonstylePoints = 0;
    }

    private void LoadSpecials(JSONObject p_Skill)
    {
        List<SpecialData> l_SpecialList = new List<SpecialData>();
        for (int i = 0; i < p_Skill.Count; i++)
        {
            l_SpecialList.Add(SpecialDataBase.GetInstance().GetSpecialData(p_Skill[i].str));
        }

        AddSkills(l_SpecialList);
    }

    private void LoadSelectedSpecials(JSONObject p_Skill)
    {
        List<SpecialData> l_SpecialList = new List<SpecialData>();
        for (int i = 0; i < p_Skill.Count; i++)
        {
            l_SpecialList.Add(SpecialDataBase.GetInstance().GetSpecialData(p_Skill[i].str));
        }

        AddSelectedSkills(l_SpecialList);
    }
    #endregion
}
