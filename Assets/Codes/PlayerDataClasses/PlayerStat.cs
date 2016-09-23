﻿using UnityEngine;
using System.Collections.Generic;

public class PlayerStat : Singleton<PlayerStat>
{
    private Dictionary<string, int> m_Stats = new Dictionary<string, int>();

    public PlayerStat()
    {
    }

    public void SetStatData(JSONObject p_JsonObject)
    {
        m_Stats.Add("HealthPoints",   (int)p_JsonObject["HealthPoints"].f);
        m_Stats.Add("MonstylePoints", (int)p_JsonObject["MonstylePoints"].f);
        m_Stats.Add("Attack",         (int)p_JsonObject["Attack"].f);
        m_Stats.Add("Defense",        (int)p_JsonObject["Defense"].f);
        m_Stats.Add("Speed",          (int)p_JsonObject["Speed"].f);
        m_Stats.Add("Fortune",        (int)p_JsonObject["Fortune"].f);
    }

    public Dictionary<string, int> GetStats()
    {
        return m_Stats;
    }

    public int GetStatValue(string p_StatName)
    {
        try
        {
            return m_Stats[p_StatName];
        }
        catch
        {
            Debug.LogError("Cannot find stat, id: " + p_StatName);
            return 0;
        }
    }

    public void AddStatValue(string p_StatName, int p_Value)
    {
        m_Stats[p_StatName] = p_Value;
    }

    public void ResetData()
    {
        m_Stats = new Dictionary<string, int>();
    }
}
