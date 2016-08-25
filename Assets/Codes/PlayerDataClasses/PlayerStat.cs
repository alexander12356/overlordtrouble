using UnityEngine;
using System.Collections.Generic;

public class PlayerStat : Singleton<PlayerStat>
{
    private Dictionary<string, int> m_Stats = new Dictionary<string, int>();

    public PlayerStat()
    {
        m_Stats.Add("HealthPoints", 20);
        m_Stats.Add("SpecialPoints", 20);
        m_Stats.Add("Attack", 1);
        m_Stats.Add("Defense", 0);
        m_Stats.Add("Speed", 1);
        m_Stats.Add("Fortune", 0);
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
}
