using UnityEngine;
using System.Collections.Generic;

public class BattleStarter : Singleton<BattleStarter>
{
    private BattleData m_BattleData;
    private JourneyActor m_Enemy;

    public void InitBattle(JourneyActor p_Enemy, string p_BattleId)
    {
        m_BattleData = BattleDataBase.GetInstance().GetBattle(p_BattleId);
        m_Enemy = p_Enemy;
    }

    public BattleData GetBattle()
    {
        return m_BattleData;
    }

    public void BattleWin()
    {
        if (m_BattleData.id != "TestBattle")
        {
            Object.Destroy(m_Enemy.gameObject);
            JourneySystem.GetInstance().SetControl(ControlType.Player);
        }
    }

    public void BattleRetreat()
    {
        if (m_BattleData.id != "TestBattle")
        {
            m_Enemy.StartLogic();
            JourneySystem.GetInstance().SetControl(ControlType.Player);
        }
    }
}
