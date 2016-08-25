using UnityEngine;
using System.Collections;

public class PlayerData : Singleton<PlayerData>
{
    private int m_CurrentLevel = 0;
    private int m_StatImprovePoints = 5;

    public int statImprovePoints
    {
        get { return m_StatImprovePoints; }
        set { m_StatImprovePoints = value; }
    }
}
