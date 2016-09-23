using UnityEngine;

using System.Collections.Generic;
using System;

public class PlayerData : Singleton<PlayerData>
{
    private string m_PathFile = "Data/PlayerData";

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

    public void ResetData()
    {
        PlayerStat.GetInstance().ResetData();
        PlayerSkills.GetInstance().ResetData();
        PlayerEnchancement.GetInstance().ResetData();
        Parse();
    }

    public PlayerData()
    {
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

    private void Parse()
    {
        string l_DecodedString = "";
        try
        {
            TextAsset l_TextAsset = (TextAsset)Resources.Load(m_PathFile);
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
        PlayerStat.GetInstance().SetStatData(l_JSONObject["Stats"]);

        m_HealthPoints = PlayerStat.GetInstance().GetStatValue("HealthPoints");
        m_MonstylePoints = PlayerStat.GetInstance().GetStatValue("HealthPoints");
    }

    private void CheckLevelup()
    {
        if (m_Levelup[m_Level] <= m_Experience)
        {
            m_Level++;
            m_StatImprovePoints += 4;
            CheckClassup();
        }
    }

    private void CheckClassup()
    {
        if (m_Classup[m_Class] < m_Level)
        {
            m_Class++;
            m_ClassImprovePoints++;
        }
    }
}
