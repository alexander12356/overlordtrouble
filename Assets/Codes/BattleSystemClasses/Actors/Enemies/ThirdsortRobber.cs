using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ThirdsortRobber : BattleEnemy
{
    private static ThirdsortRobber m_Prefab = null;
    private int   m_StealChance = 10;
    private int   m_StealMonettCount = 4;

    public static ThirdsortRobber prefab
    {
        get
        {
            if (m_Prefab == null)
            {
                m_Prefab = Resources.Load<ThirdsortRobber>("Prefabs/BattleEnemies/ThirdsortRobber");
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

        int l_RunChance = l_Random.Next(0, 100);

        if (TurnSystem.GetInstance().currentTurn > 0 && l_RunChance > 50)
        {
            Run();
        }
        else
        {
            Attack(BattlePlayer.GetInstance());

            int l_RandomStealChance = l_Random.Next(0, 100);
            if (PlayerInventory.GetInstance().coins > m_StealMonettCount && l_RandomStealChance > m_StealChance)
            {
                PlayerInventory.GetInstance().coins -= m_StealMonettCount;

                string l_TextAboutКгт = LocalizationDataBase.GetInstance().GetText("Enemy:ThirdsortRobber:StealMonett", new string[] { m_StealMonettCount.ToString() });
                TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
                l_TextPanel.SetText(new List<string>() { l_TextAboutКгт });
                l_TextPanel.AddButtonAction(l_TextPanel.Close);

                BattleShowPanelStep l_ShowPanelStep = new BattleShowPanelStep(l_TextPanel);
                ResultSystem.GetInstance().AddStep(l_ShowPanelStep);
            }
        }
        ResultSystem.GetInstance().ShowResult();
    }

    public void Run()
    {
        LeshiiAttackEffect l_RunVisualEffect = Instantiate(LeshiiAttackEffect.prefab);
        l_RunVisualEffect.AddPlayAction(PlayRunAnimation);

        BattlePlayEffectStep l_PlayEffectStep = new BattlePlayEffectStep(l_RunVisualEffect);
        ResultSystem.GetInstance().AddStep(l_PlayEffectStep);

        string l_TextAboutКгт = LocalizationDataBase.GetInstance().GetText("Enemy:ThirdsortRobber:Run");
        TextPanel l_TextPanel = Instantiate(TextPanel.prefab);
        l_TextPanel.SetText(new List<string>() { l_TextAboutКгт });
        l_TextPanel.AddButtonAction(l_TextPanel.Close);

        BattleShowPanelStep l_ShowPanelStep = new BattleShowPanelStep(l_TextPanel);
        ResultSystem.GetInstance().AddStep(l_ShowPanelStep);
    }

    //Called from Animation
    public void Runned()
    {
        BattleSystem.GetInstance().EnemyDied(this);

        Destroy(gameObject);

        ResultSystem.GetInstance().NextStep();
    }

    private void PlayRunAnimation()
    {
        m_Animator.SetTrigger("Run");
    }
}
