using UnityEngine;
using System.Collections.Generic;

public class BattleStarter : Singleton<BattleStarter>
{
    private BattleData m_BattleData;
    private JourneyEnemy m_Enemy;

    public void InitBattle(JourneyEnemy p_Enemy, string p_BattleId)
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
        if (!m_BattleData.id.Contains("TestBattle"))
        {
            JourneySystem.GetInstance().SetControl(ControlType.Player);
            m_Enemy.Lose();
        }
    }

    public void BattleRetreat()
    {
        if (!m_BattleData.id.Contains("TestBattle"))
        {
            m_Enemy.StartLogic();
            JourneySystem.GetInstance().SetControl(ControlType.Player);
            m_Enemy.Win();
        }
    }
}
