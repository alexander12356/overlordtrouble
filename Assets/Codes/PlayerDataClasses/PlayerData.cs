using UnityEngine;
using System.Collections;

public class PlayerData : Singleton<PlayerData>
{
    private int m_CurrentLevel = 0;
    private int m_StatImprovePoints = 5;
    private int m_Health = 20;

    public int health
    {
        get { return m_Health;  }
        set { m_Health = value; }
    }
    public int statImprovePoints
    {
        get { return m_StatImprovePoints; }
        set { m_StatImprovePoints = value; }
    }

    public PlayerData()
    {
        m_Health = PlayerStat.GetInstance().GetStatValue("HealthPoints");
    }
}
