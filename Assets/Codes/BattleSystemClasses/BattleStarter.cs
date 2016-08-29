using UnityEngine;
using System.Collections.Generic;

public class BattleStarter : Singleton<BattleStarter>
{
    private List<string> m_EnemyIds = new List<string>();

    public void AddEnemy(string p_EnemyId)
    {
        m_EnemyIds.Add(p_EnemyId);
    }

    public void SetPlayerPosition(Vector3 p_PlayerPosition)
    {

    }

    public List<string> GetEnemy(bool p_WithRemoveList = true)
    {
        List<string> l_EnemyList = new List<string>();

        // TODO kostill
        for (int i = 0; i < m_EnemyIds.Count; i++)
        {
            l_EnemyList.Add(m_EnemyIds[i]);
        }

        if (p_WithRemoveList)
        {
            m_EnemyIds.Clear();
        }
        return l_EnemyList;
    }
}
