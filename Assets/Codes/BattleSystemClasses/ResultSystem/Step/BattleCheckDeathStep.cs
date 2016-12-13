using System.Collections.Generic;

using UnityEngine;

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

            string l_TextAboutDeath = LocalizationDataBase.GetInstance().GetText("GUI:BattleSystem:EnemyDied", new string[] { m_DeathEnemyList[i].actorName });

            TextPanel l_TextPanel = Object.Instantiate(TextPanel.prefab);
            l_TextPanel.SetText(new List<string>() { l_TextAboutDeath });
            l_TextPanel.AddButtonAction(l_TextPanel.Close);

            BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
            ResultSystem.GetInstance().AddStep(l_Step);
        }
    }
}
