using System;
using System.Collections.Generic;

using UnityEngine;

public class SecondRateRobber : BattleEnemy
{
    private static SecondRateRobber m_Prefab = null;
    private int m_StealChance = 10;
    private int m_StealMonettCount = 4;

    public static SecondRateRobber prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<SecondRateRobber>("Prefabs/BattleEnemies/SecondRateRobber");
            }
            return m_Prefab;
        }
    }

    public override void InitStats()
    {
        base.InitStats();

        m_StealChance = Convert.ToInt32(m_EnemyData.property[0]);
        m_StealMonettCount = Convert.ToInt32(m_EnemyData.property[1]);
    }

    public override void RunTurn()
    {
        System.Random l_Random = new System.Random();

        Attack(BattlePlayer.GetInstance());

        int l_RandomStealChance = l_Random.Next(0, 100);
        if (PlayerInventory.GetInstance().coins > m_StealMonettCount && l_RandomStealChance > m_StealChance)
        {
            PlayerInventory.GetInstance().coins -= m_StealMonettCount;

            string l_Text = LocalizationDataBase.GetInstance().GetText("Enemy:ThirdsortRobber:StealMonett", new string[] { m_StealMonettCount.ToString() });
            TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
            l_TextPanel.SetText(new List<string>() { l_Text });
            l_TextPanel.AddButtonAction(l_TextPanel.Close);

            BattleShowPanelStep l_ShowPanelStep = new BattleShowPanelStep(l_TextPanel);
            ResultSystem.GetInstance().AddStep(l_ShowPanelStep);
        }
        ResultSystem.GetInstance().ShowResult();
    }
}
