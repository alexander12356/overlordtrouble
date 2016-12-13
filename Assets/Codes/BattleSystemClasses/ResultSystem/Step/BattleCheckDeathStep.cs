using System.Collections.Generic;

public class BattleCheckDeathStep : BattleBaseStep
{
    private List<BattleEnemy> m_DeathEnemyList = new List<BattleEnemy>();

    public BattleCheckDeathStep()
    {
    }

    public override void RunStep()
    {
        base.RunStep();

        List<BattleEnemy> l_EnemyList = BattleSystem.GetInstance().GetEnemyList();
        for (int i = 0; i < l_EnemyList.Count; i++)
        {
            if (l_EnemyList[i].CheckDeath())
            {
                m_DeathEnemyList.Add(l_EnemyList[i]);
            }
        }

        ShowDeathActors();

        ResultSystem.GetInstance().NextStep();
    }

    private void ShowDeathActors()
    {
        for (int i = 0; i < m_DeathEnemyList.Count; i++)
        {
            m_DeathEnemyList[i].Die();
        }
    }
}
