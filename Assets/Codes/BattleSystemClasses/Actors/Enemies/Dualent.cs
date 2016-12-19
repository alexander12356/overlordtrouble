using System;
using System.Collections.Generic;

using UnityEngine;

public class Dualent : BattleEnemy
{
    private static Dualent m_Prefab = null;
    private float l_DoubleAttackChanse = 0.0f;

    public static Dualent prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<Dualent>("Prefabs/BattleEnemies/Dualent");
            }
            return m_Prefab;
        }
    }

    public override void RunTurn()
    {
        if (UnityEngine.Random.Range(0, 100) < l_DoubleAttackChanse)
        {
            Attack(BattlePlayer.GetInstance());

            TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
            string l_Text = LocalizationDataBase.GetInstance().GetText("Enemy:Dualent:DoubleAttack");
            l_TextPanel.SetText(new List<string>() { l_Text });
            l_TextPanel.AddButtonAction(l_TextPanel.Close);

            BattleShowPanelStep l_Step = new BattleShowPanelStep(l_TextPanel);
            ResultSystem.GetInstance().AddStep(l_Step);

            Attack(BattlePlayer.GetInstance());

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

        l_DoubleAttackChanse = Convert.ToSingle(m_EnemyData.property[0]);
    }
}
