using System.Collections.Generic;

public class DeathOrderSort : IComparer<BattleEnemy>
{
    public int Compare(BattleEnemy x, BattleEnemy y)
    {
        if (x.deathOrder > y.deathOrder)
        {
            return 1;
        }
        else if (x.deathOrder < y.deathOrder)
        {
            return -1;
        }
        return 0;
    }
}

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

        if (BattlePlayer.GetInstance().CheckDeath())
        {
            BattlePlayer.GetInstance().Die();
        }

        ResultSystem.GetInstance().NextStep();
    }

    private void ShowDeathActors()
    {
        if (m_DeathEnemyList.Count == 0)
        {
            return;
        }

        DeathOrderSort l_DeathOrderSort = new DeathOrderSort();
        m_DeathEnemyList.Sort(l_DeathOrderSort);
        for (int i = 0; i < m_DeathEnemyList.Count; i++)
        {
            m_DeathEnemyList[i].Die();
        }
    }
}
