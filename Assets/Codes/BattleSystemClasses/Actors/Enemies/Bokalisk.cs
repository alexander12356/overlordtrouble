using System;
using System.Collections.Generic;

using UnityEngine;

public class Bokalisk : BattleEnemy
{
    private static Bokalisk m_Prefab = null;
    private float l_MissTurnChance = 0.0f;

    public static Bokalisk prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<Bokalisk>("Prefabs/BattleEnemies/Bokalisk");
            }
            return m_Prefab;
        }
    }

    public override void RunTurn()
    {
        if (UnityEngine.Random.Range(0, 100) < l_MissTurnChance)
        {
            TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
            string l_Text = LocalizationDataBase.GetInstance().GetText("Enemy:Bokalisk:MissTurn");
            l_TextPanel.SetText(new List<string>() { l_Text });
            l_TextPanel.AddButtonAction(l_TextPanel.Close);

            BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
            ResultSystem.GetInstance().AddStep(l_Step);

            ResultSystem.GetInstance().ShowResult();
        }
        else
        {
            base.RunTurn();
        }
    }

    public override void InitStats()
    {
        base.InitStats();

        l_MissTurnChance = Convert.ToSingle(m_EnemyData.property[0]);
    }
}
