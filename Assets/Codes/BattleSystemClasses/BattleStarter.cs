using UnityEngine;
using System.Collections.Generic;

public class BattleStarter : Singleton<BattleStarter>
{
    private BattleData m_BattleData;

    public void InitBattle(string p_BattleId)
    {
        m_BattleData = BattleDataBase.GetInstance().GetBattle(p_BattleId);
    }

    public BattleData GetBattle()
    {
        return m_BattleData;
    }
}
